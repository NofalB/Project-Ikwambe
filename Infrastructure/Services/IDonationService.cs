using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDonationService
    {
        IEnumerable<Donation> GetAllDonations();

        Task<Donation> GetDonationById(string donationId);

        Task AddDonation(Donation donation);

    }
}