using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ICosmosRepository<User> _userRepository;

        public UserService(ICosmosRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            Guid id = Guid.Parse(userId);
            return await _userRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == id);
        }

        private async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> AddUser(UserDTO userDTO)
        {
            if(await GetUserByEmail(userDTO.Email) == null)
            {
                User user = new User(
                Guid.NewGuid(),
                userDTO.FirstName,
                userDTO.LastName,
                userDTO.Email,
                userDTO.Password,
                false
                );

                return await _userRepository.AddAsync(user);
            }
            else
            {
                throw new Exception("The user email already exist");
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            return await _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            User user = await GetUserById(userId);
            await _userRepository.Delete(user);
        }
    }
}
