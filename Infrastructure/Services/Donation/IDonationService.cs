using Domain;
using Domain.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDonationService
    {

        Task<Donation> GetDonationByIdAsync(string donationId);

        Task<Donation> AddDonation(DonationDTO donation);

        Task<Donation> GetDonationByUserIdAsync(string userId);

        Task<List<Donation>> GetDonationByQueryOrGetAllAsync(string projectId, string donationDate);
    }
}