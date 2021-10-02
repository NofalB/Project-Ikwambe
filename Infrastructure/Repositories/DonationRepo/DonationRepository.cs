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
            return _ikambeContext.Donations;
        }

        public override async Task<Donation> GetByIdAsync(int id)
        {
            return await _ikambeContext.Donations.FindAsync(id);
        }
    }
}
