﻿using Domain;
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
            return await _donationReadRepository.GetAll().ToListAsync();
        }

        public async Task<Donation> GetDonationByIdAsync(string donationId)
        {
            Guid id = Guid.Parse(donationId);
            return await _donationReadRepository.GetAll().FirstOrDefaultAsync(d => d.DonationId == id);
        }

        public IQueryable<Donation> GetDonationByQueryOrGetAll(string userId, string projectId, string date)
        {
            IQueryable<Donation> donation = _donationReadRepository.GetAll();
            
            if (userId != null) 
            {
                donation = donation.Where(d => d.UserId == Guid.Parse(userId));
            }
            if(projectId != null)
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
