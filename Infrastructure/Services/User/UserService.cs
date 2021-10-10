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
            return await _userRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            string newId = Guid.NewGuid().ToString();
            User user = new User()
            {
                UserId = newId,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Subscription = false,
                PartitionKey = newId
            };

            return await _userRepository.AddAsync(user);
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
