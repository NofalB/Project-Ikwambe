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
            return await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userId));
        }

        private async Task<User> GetUserByEmail(string email)
        {
            return await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
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

        public List<User> GetUserByQueryOrGetAll(string firstname, string lastname, string subcribe)
        {
            List<User> users = _userReadRepository.GetAll().ToList();
            List<User> resultList = new List<User>();

            if (firstname != null)
            {
                resultList.AddRange(users.Where(f =>
                {
                    try
                    {
                        return f.FirstName == firstname;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"No user with the firstname {firstname} has been found.");
                    }
                }));
                //user = user.Where(f => f.FirstName == firstname);
            }
            if (lastname != null)
            {
                resultList.AddRange(users.Where(l =>
                {
                    try
                    {
                        return l.LastName == lastname;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"No user with the firstname {lastname} has been found.");
                    }
                }));
                //user = user.Where(l => l.LastName == lastname);
            }
            if (subcribe != null)
            {
                resultList.AddRange(users.Where(s =>
                {
                    try
                    {
                        return s.Subscription == bool.Parse(subcribe);
                    }
                    catch
                    {
                        throw new InvalidOperationException("Please either use true or false");
                    }
                }));
                //user = user.Where(s => s.Subscription == bool.Parse(subcribe));
            }

            return resultList.Count != 0 ? resultList: users;
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            if(await GetUserByEmail(userDTO.Email) == null)
            {
                User user = new User()
                { 
                    UserId = Guid.NewGuid(),
                    FirstName = !string.IsNullOrEmpty(userDTO.FirstName) ? userDTO.FirstName : throw new ArgumentNullException($"Invalid {nameof(userDTO.FirstName)} provided"),
                    LastName = !string.IsNullOrEmpty(userDTO.LastName) ? userDTO.LastName : throw new ArgumentNullException($"Invalid {nameof(userDTO.LastName)} provided"),
                    Email = !string.IsNullOrEmpty(userDTO.Email) ? userDTO.Email : throw new ArgumentNullException($"Invalid {nameof(userDTO.Email)} provided"),
                    Password = !string.IsNullOrEmpty(userDTO.Password) ? userDTO.Password : throw new ArgumentNullException($"Invalid {nameof(userDTO.Password)} provided"),
                    Subscription = false,
                    Role = Role.User,
                    PartitionKey =  userDTO.FirstName
                    };

                return await _userWriteRepository.AddAsync(user);
            }
            else
            {
                throw new Exception("The user email already exist");
            }
        }

        public async Task<User> UpdateUser(User user, string userId)
        {
            if(await GetUserById(userId) == null)
            {
                throw new InvalidOperationException("The user ID provided does not exist.");
            }
            return await _userWriteRepository.Update(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            User user = await GetUserById(userId);
            await _userWriteRepository.Delete(user);
        }
    }
}
