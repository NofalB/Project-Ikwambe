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

        public DonationService(CosmosRepository<Donation> donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public IEnumerable<Donation> GetAllDonations()
        {
            return _donationRepository.GetAll().ToList();
        }

        public Donation GetDonationById(string donationId)
        {
            return _donationRepository.GetById(donationId);
        }

        public async Task AddDonation(Donation donation)
        {
            await _donationRepository.AddAsync(donation);
        }
    }
}
