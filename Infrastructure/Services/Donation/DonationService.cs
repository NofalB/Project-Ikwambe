using Domain;
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

        public async Task<Donation> AddDonation(Donation donation)
        {
            return await _donationRepository.AddAsync(donation);
        }
    }
}
