using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepo
{
    public class UserRepository : CosmosRepository<User>
    {
        public UserRepository(IkwambeContext ikambeContext) : base(ikambeContext)
        {

        }

        public override IEnumerable<User> GetAll()
        {
            return _ikambeContext.Users;
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _ikambeContext.Users.FindAsync(id);
        }
    }
}
