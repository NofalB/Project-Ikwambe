using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly ICosmosReadRepository<Donation> _donationReadRepository;
        private readonly ICosmosWriteRepository<Donation> _donationWriteRepository;
        
        private readonly IUserService _userService;
        private readonly IWaterpumpProjectService _waterpumpProjectService;

        public DonationService(ICosmosReadRepository<Donation> donationReadRepository, ICosmosWriteRepository<Donation> donationWriteRepository,
            IUserService userService, IWaterpumpProjectService waterpumpProjectService)
        {
            _donationReadRepository = donationReadRepository;
            _donationWriteRepository = donationWriteRepository;
            _userService = userService;
            _waterpumpProjectService = waterpumpProjectService;
        }

        public async Task<Donation> GetDonationByIdAsync(string donationId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(donationId) ? Guid.Parse(donationId) : throw new ArgumentNullException("No donation ID was provided.");

                var donation = await _donationReadRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id);

                if (donation == null)
                {
                    throw new InvalidOperationException($"Donation does not exist. Incorrect donation ID {donationId} provided");
                }
                return donation;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid donation ID {donationId} provided.");
            }
        }

        public async Task<List<Donation>> GetDonationByUserIdAsync(string userId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : throw new ArgumentNullException("No user Id was provided.");

                var donations = await _donationReadRepository.GetAll().Where(d => d.UserId == id).ToListAsync();
                return donations;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid user Id {userId} provided.");
            }
        }

        public async Task<List<Donation>> GetDonationByQueryOrGetAllAsync(string projectId, string donationDate)
        {
            List<Donation> resultsList = new List<Donation>();
            List<Donation> donations = _donationReadRepository.GetAll().ToList();
            

            if (!string.IsNullOrEmpty(projectId))
            {
                var waterpumpProject = await _waterpumpProjectService.GetWaterPumpProjectById(projectId) ?? throw new InvalidOperationException($"Project {projectId} does not exist.");

                resultsList = donations.Where(d => d.ProjectId == Guid.Parse(projectId)).ToList();
            }
            if(!string.IsNullOrEmpty(donationDate))
            {
                DateTime donationDt;

                if (!DateTime.TryParseExact(donationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out donationDt))
                {
                    throw new InvalidOperationException("Invalid date provided.");
                }
                donationDt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                resultsList = resultsList.Count != 0 ? 
                    donations.Where(d => d.DonationDate.Date == donationDt.Date && d.ProjectId.ToString() == projectId).ToList()
                    : resultsList = donations.Where(d => d.DonationDate.Date == donationDt.Date).ToList();

                return resultsList.Count != 0 ? resultsList : new List<Donation>();
            }

            return resultsList.Count != 0 ? resultsList : donations;
        }

        public async Task<Donation> AddDonation(DonationDTO donationDTO)
        {
            if (donationDTO == null)
            {
                throw new NullReferenceException($"{nameof(DonationDTO)} cannot be null.");
            }

            if (donationDTO.UserId != null)
            {
                var user = await _userService.GetUserById(donationDTO.UserId.ToString());
                if (user.Role == Role.Admin)
                {
                    throw new InvalidOperationException("Admin user cannot make donations.");
                }
            }

            if (donationDTO.ProjectId != Guid.Empty)
            {
                var waterpumpProject = await _waterpumpProjectService.GetWaterPumpProjectById(donationDTO.ProjectId.ToString()) ?? throw new InvalidOperationException($"Project {donationDTO.ProjectId} does not exist.");
                
                Donation donation = new Donation()
                {
                    DonationId = Guid.NewGuid(),
                    UserId = donationDTO.UserId,
                    ProjectId = donationDTO.ProjectId != Guid.Empty ? donationDTO.ProjectId : throw new InvalidOperationException($"Invalid {nameof(donationDTO.ProjectId)} provided."),
                    TransactionId = donationDTO.TransactionId ?? throw new ArgumentNullException($"Invalid {nameof(donationDTO.TransactionId)} provided"),
                    Amount = donationDTO.Amount != 0 ? donationDTO.Amount : throw new InvalidOperationException($"Invalid {nameof(donationDTO.Amount)} provided."),
                    DonationDate = donationDTO.DonationDate != default(DateTime) ? donationDTO.DonationDate : throw new InvalidOperationException($"Invalid {nameof(donationDTO.DonationDate)} provided."),
                    PartitionKey = donationDTO.ProjectId.ToString() ?? throw new ArgumentNullException($"Invalid {nameof(donationDTO.ProjectId)} provided")
                };
                return await _donationWriteRepository.AddAsync(donation);
            }

            throw new ArgumentNullException($"Invalid {nameof(donationDTO.ProjectId)} provided.");
        }
    }
}
