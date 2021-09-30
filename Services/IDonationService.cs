using ProjectIkwambe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectIkwambe.Services
{
    public interface IDonationService
    {
        IEnumerable<Donation> GetAllDonations();

        Donation GetDonation();

        Task AddDonation(Donation donation);

    }
}