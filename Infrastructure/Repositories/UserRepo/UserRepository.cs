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
            try
            {
                return _ikambeContext.Users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users. {ex.Message}");
            }
        }

        public override User GetById(string userId)
        {
            try
            {
                return _ikambeContext.Users.FirstOrDefault(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user {userId}. {ex.Message}");
            }
        }

    }
}
