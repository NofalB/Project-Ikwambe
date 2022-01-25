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

        Task<User> GetUserById(string userId, bool fullData = false);

        Task<User> AddUser(UserDTO userDTO);

        Task<User> UpdateUser(UserDTO user, string userId);

        Task DeleteUserAsync(string userId);

        List<UserResponseDTO> GetUserByQueryOrGetAll(string firstname, string lastname, string subcribe);

        User UserCheck(string email, string password);

        Task<User> UpdateUserRoleToAdmin(string userId);
    }
}
