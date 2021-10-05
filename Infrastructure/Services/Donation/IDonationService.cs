using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDonationService
    {
        Task<List<Donation>> GetAllDonationsAsync();

        Task<Donation> GetDonationByIdAsync(string donationId);

        Task AddDonation(Donation donation);

    }
}