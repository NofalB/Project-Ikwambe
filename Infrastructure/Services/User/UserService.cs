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
        private readonly ICosmosReadRepository<User> _userReadRepository;
        private readonly ICosmosWriteRepository<User> _userWriteRepository;

        public UserService(ICosmosReadRepository<User> userReadRepository, ICosmosWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userReadRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            Guid id = Guid.Parse(userId);
            return await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == id);
        }

        private async Task<User> GetUserByEmail(string email)
        {
            return await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
        }

        private IQueryable<User> GetUserByFirstName(string firstName)
        {
            return _userReadRepository.GetAll().Where(u => u.FirstName == firstName);
        }

        private IQueryable<User> GetUserByLastName(string lastName)
        {
            return _userReadRepository.GetAll().Where(u => u.LastName == lastName);
        }

        private IQueryable<User> GetListOfUserSubscription(string subscribe)
        {
            bool isSubscribe = bool.Parse(subscribe);
            return _userReadRepository.GetAll().Where(u => u.Subscription == isSubscribe);
        }

        public User UserCheck(string email, string password)
        {
            User user = _userReadRepository.GetAll().FirstOrDefault(u => u.Email == email);
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

            IQueryable<User> user = _userReadRepository.GetAll();

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

                return await _userWriteRepository.AddAsync(user);
            }
            else
            {
                throw new Exception("The user email already exist");
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            return await _userWriteRepository.Update(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            User user = await GetUserById(userId);
            await _userWriteRepository.Delete(user);
        }
    }
}
