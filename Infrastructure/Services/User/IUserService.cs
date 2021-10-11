using Domain;
using Domain.DTO;
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

        Task<User> GetUserById(Guid userId);

        Task<User> AddUser(UserDTO userDTO);

        Task<User> UpdateUser(User user);

        Task DeleteUserAsync(Guid userId);
    }
}
