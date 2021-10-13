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

/*        private IQueryable<User> GetUserByFirstName(string firstName)
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
        }*/

        public User UserCheck(string email, string password)
        {
            User user = _userRepository.GetAll().FirstOrDefault(u => u.Email == email);
            if(user== null)
            {
                throw new Exception("The email you have provided does not exist");
            }
            else
            {
                if (user.Password == password)
                {
                    return user;
                }
                else
                {
                    throw new Exception("Please check your credentials");
                }
            }
        }

        public IQueryable<User> GetUserByQueryOrGetAll(string firstname, string lastname, string subcribe)
        {

            IQueryable<User> user = _userRepository.GetAll();

            if (firstname != null)
            {
                user = user.Where(f => f.FirstName == firstname);
            }
            if (lastname != null)
            {
                user = user.Where(l => l.LastName == lastname);
            }
            if (subcribe != null)
            {
                user = user.Where(s => s.Subscription == bool.Parse(subcribe));
            }

            return user;
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
