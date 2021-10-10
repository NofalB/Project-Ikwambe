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
        private readonly ICosmosRepository<Donation> _donationRepository;

        public DonationService(ICosmosRepository<Donation> donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public async Task<IEnumerable<Donation>> GetAllDonationsAsync()
        {
            return await _donationRepository.GetAll().ToListAsync();
        }

        public async Task<Donation> GetDonationByIdAsync(string donationId)
        {
            return await _donationRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == donationId);
        }

        public async Task<Donation> AddDonation(DonationDTO donationDTO)
        {
            string newId = Guid.NewGuid().ToString();
            Donation donation = new Donation()
            {
                DonationId = newId,
                UserId = donationDTO.UserId,
                ProjectId = donationDTO.ProjectId,
                TransactionId = donationDTO.TransactionId,
                Amount = donationDTO.Amount,
                DonationDate = donationDTO.DonationDate,
                PartitionKey = donationDTO.ProjectId
            };

            return await _donationRepository.AddAsync(donation);
        }
    }
}
