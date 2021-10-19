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

        public DonationService(ICosmosReadRepository<Donation> donationReadRepository, ICosmosWriteRepository<Donation> donationWriteRepository)
        {
            _donationReadRepository = donationReadRepository;
            _donationWriteRepository = donationWriteRepository;
        }

        public async Task<IEnumerable<Donation>> GetAllDonationsAsync()
        {
            //here a check. for user 
            return await _donationReadRepository.GetAll().ToListAsync();
        }

        public async Task<Donation> GetDonationByIdAsync(string donationId)
        {
            try
            {
                Guid id = Guid.Parse(donationId);

                var donation = await _donationReadRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id);

                if (donation == null)
                {
                    throw new ArgumentException("Donation does not exist. Incorrect donation ID or user ID provided");
                }
                return donation;
            }
            catch
            {
                throw new InvalidOperationException("Invalid donation ID provided.");
            }
        }

        public IQueryable<Donation> GetDonationByUserId(string userId)
        {
            return _donationReadRepository.GetAll().Where(d=> d.UserId == Guid.Parse(userId));
        }

        public List<Donation> GetDonationByQueryOrGetAll(string projectId, string donationDate)
        {
            List<Donation> resultsList = new List<Donation>();
            List<Donation> donations = _donationReadRepository.GetAll().ToList();
            
            if (projectId != null)
            {
                resultsList.AddRange(donations.Where(d =>
                {
                    try
                    {
                        return d.ProjectId == Guid.Parse(projectId);
                    }
                    catch
                    {
                        throw new InvalidOperationException("Invalid Project ID provided.");
                    }
                }));
            }
            if(donationDate != null)
            {
                DateTime donationDt;

                if (!DateTime.TryParseExact(donationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out donationDt))
                {
                    throw new InvalidOperationException("Invalid date provided.");
                }
                
                donationDt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                resultsList.AddRange(donations.Where(d => d.DonationDate.Date == donationDt.Date).ToList());
            }

            return resultsList.Count != 0 ? resultsList : donations;
        }

        public async Task<Donation> AddDonation(DonationDTO donationDTO)
        {
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
    }
}
