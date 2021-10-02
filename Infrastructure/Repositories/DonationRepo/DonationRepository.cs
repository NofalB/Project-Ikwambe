using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.DonationRepo
{
    public class DonationRepository : CosmosRepository<Donation>
    {
        public DonationRepository(IkwambeContext ikambeContext) : base(ikambeContext)
        {

        }

        public override IEnumerable<Donation> GetAll()
        {
            try
            {
                return _ikambeContext.Donations;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving donations. {ex.Message}");
            }
        }


        public override Donation GetById(string donationId)
        {
            try
            {
                return _ikambeContext.Donations.FirstOrDefault(d => d.DonationId == donationId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving donation {donationId}. {ex.Message}");
            }
        }
    }
}
