using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly CosmosRepository<User> _userRepository;

        public UserService(CosmosRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public User GetUserById(string userId)
        {
            //return _userRepository.GetById(userId);
            return null;
        }

        public async Task AddUser(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public User UpdateUser(User user)
        {
            return _userRepository.Update(user);
        }

        public void DeleteUser(string userId)
        {
            User user = GetUserById(userId);
            _userRepository.Delete(user);
        }
    }
}
