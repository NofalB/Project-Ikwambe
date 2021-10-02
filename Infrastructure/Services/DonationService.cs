using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly CosmosRepository<Donation> _donationRepository;

        public DonationService(CosmosRepository<Donation> cosmosRepository)
        {
            _donationRepository = cosmosRepository;
        }

        public IEnumerable<Donation> GetAllDonations()
        {
            return _donationRepository.GetAll().ToList();
        }

        public async Task AddDonation(Donation donation)
        {
            await _donationRepository.AddAsync(donation);
        }

        public async Task<Donation> GetDonationById(string donationId)
        {
            return await _donationRepository.GetByIdAsync(donationId);
        }
    }
}
