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
        private readonly ICosmosRepository<Donation> _cosmosRepository;

        public DonationService(ICosmosRepository<Donation> cosmosRepository)
        {
            _cosmosRepository = cosmosRepository;
        }

        public IEnumerable<Donation> GetAllDonations()
        {
            throw new NotImplementedException();
        }

        public async Task AddDonation(Donation donation)
        {
            await _cosmosRepository.AddAsync(donation);
        }

        public Donation GetDonation()
        {
            throw new NotImplementedException();
        }
    }
}
