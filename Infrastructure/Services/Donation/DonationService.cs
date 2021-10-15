using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Guid id = Guid.Parse(donationId);
            return await _donationReadRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id);
        }

        public async Task<Donation> GetDonationByIdAsync(string donationId, string userId)
        {
            Guid id = Guid.Parse(donationId);
            
            var donation  = await _donationReadRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id && d.UserId == Guid.Parse(userId));
            
            if(donation == null)
            {
                throw new Exception("Donation does not exist. Incorrect donation ID or user ID provided");
            }

            return donation;
        }

        public IQueryable<Donation> GetDonationByUserId(string userId)
        {
            return _donationReadRepository.GetAll().Where(d=> d.UserId == Guid.Parse(userId));
        }

        public IQueryable<Donation> GetDonationByQueryOrGetAll(string projectId, string date)
        {
            //need to fix this
            IQueryable<Donation> donation = _donationReadRepository.GetAll();
            
            if (projectId != null)
            {
                donation = donation.Where(d => d.ProjectId == Guid.Parse(projectId));
            }
            if(date != null)
            {
                DateTime time = DateTime.Parse(date);
                date += "T23:59:59";
                DateTime time_end = DateTime.Parse(date);
                donation = donation.Where(d => d.DonationDate > time && d.DonationDate < time_end);
            }

            return donation;
        }

        public async Task<Donation> AddDonation(DonationDTO donationDTO)
        {
            Donation donation = new Donation()
            {
                DonationId = Guid.NewGuid(),
                UserId = donationDTO.UserId,
                ProjectId = donationDTO.ProjectId,
                TransactionId = donationDTO.TransactionId,
                Amount = donationDTO.Amount,
                DonationDate = donationDTO.DonationDate,
                PartitionKey = donationDTO.ProjectId.ToString()
            };

            return await _donationWriteRepository.AddAsync(donation);
        }
    }
}
