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
        private readonly ICosmosRepository<DonationDTO> _donationDTORepository;

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

        public async Task<DonationDTO> AddDonation(DonationDTO donationDTO)
        {
            return await _donationDTORepository.AddAsync(donationDTO);
        }
    }
}
