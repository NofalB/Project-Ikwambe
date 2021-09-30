using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDonationService
    {
        IEnumerable<Donation> GetAllDonations();

        Donation GetDonation();

        Task AddDonation(Donation donation);

    }
}