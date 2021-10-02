using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(string userId);

        Task AddUser(User user);

        User UpdateUser(User user);

        void DeleteUser(string userId);
    }
}
