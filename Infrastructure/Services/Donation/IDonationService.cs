using Domain;
using Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDonationService
    {
        Task<IEnumerable<Donation>> GetAllDonationsAsync();

        Task<Donation> GetDonationByIdAsync(string donationId);

        Task<Donation> AddDonation(DonationDTO donation);

    }
}