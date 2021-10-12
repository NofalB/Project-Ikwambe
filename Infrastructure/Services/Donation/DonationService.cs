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
            Guid id = Guid.Parse(donationId);
            return await _donationRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id);
        }

        public IQueryable<Donation> GetDonationByQueryOrGetAll(string userId, string projectId)
        {
            if(userId != null && projectId != null)
            {
                return _donationRepository.GetAll().Where(d => d.ProjectId == Guid.Parse(projectId)).Where(u => u.UserId == Guid.Parse(userId));
            }
            else if(userId == null && projectId != null)
            {
                return _donationRepository.GetAll().Where(d => d.ProjectId == Guid.Parse(projectId));
            }
            else if(userId != null && projectId == null)
            {
                return _donationRepository.GetAll().Where(d => d.UserId == Guid.Parse(userId));
            }
            else
            {
                return _donationRepository.GetAll();
            }
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

            return await _donationRepository.AddAsync(donation);
        }
    }
}
