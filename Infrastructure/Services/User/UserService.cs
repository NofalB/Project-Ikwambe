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

        private IQueryable<User> GetUserByFirstName(string firstName)
        {
            return _userRepository.GetAll().Where(u => u.FirstName == firstName);
        }

        private IQueryable<User> GetUserByLastName(string lastName)
        {
            return _userRepository.GetAll().Where(u => u.LastName == lastName);
        }

        private IQueryable<User> GetListOfUserSubscription(string subscribe)
        {
            bool isSubscribe = bool.Parse(subscribe);
            return _userRepository.GetAll().Where(u => u.Subscription == isSubscribe);
        }

        public IQueryable<User> GetUserByQueryOrGetAll(string firstname, string lastname, string subcribe)
        {
            if (firstname != null && lastname != null && subcribe != null)
            {
                return _userRepository.GetAll().Where(f => f.FirstName == firstname).Where(l => l.LastName == lastname).Where(s => s.Subscription == bool.Parse(subcribe));
            }
            else if (firstname != null && lastname == null && subcribe == null) 
            {
                return GetUserByFirstName(firstname);
            }
            else if (firstname == null && lastname != null && subcribe == null)
            {
                return GetUserByLastName(lastname);
            }
            else if (firstname == null && lastname == null && subcribe != null)
            {
                return GetListOfUserSubscription(subcribe);
            }
            else
            {
                return _userRepository.GetAll();
            }
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
