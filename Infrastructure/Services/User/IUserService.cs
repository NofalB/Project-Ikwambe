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
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(string userId);

        Task<User> AddUser(User user);

        Task<User> UpdateUser(User user);

        Task DeleteUserAsync(string userId);
    }
}
