using BCrypt.Net;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

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

        public async Task<User> GetUserById(string userId, bool fullData = false)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : throw new ArgumentNullException("No user ID was provided.");

                var user = await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    throw new InvalidOperationException($"Invalid User ID {userId} provided.");
                }

                return fullData == true ? user : new UserResponseDTO(user.UserId, user.FirstName, user.LastName, user.Subscription);
            }
            catch
            {
                throw new InvalidOperationException($"Invalid User ID {userId} provided.");
            }
        }

        private async Task<User> GetUserByEmail(string email)
        {
            email  =  !string.IsNullOrEmpty(email) ? email : throw new ArgumentNullException("No email was provided.");
            var user = await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                throw new InvalidOperationException($"The User Email {user.Email} provided already exist");
            }
            return user;
        }

        public User UserCheck(string email, string password)
        {
            User user = _userReadRepository.GetAll().FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new InvalidOperationException("Please check your credentials");
            }
            else
            {
                if  (BCryptNet.Verify(password, user.Password))
                {
                    return user;
                }
                else
                {
                    throw new InvalidOperationException("Please check your credentials");
                }
            }
        }

        public List<UserResponseDTO> GetUserByQueryOrGetAll(string firstname, string lastname, string subcribe)
        {
            var resultList = new List<UserResponseDTO>();
            _userReadRepository.GetAll().ToList()
            .ForEach(
                u => resultList.Add(
                    new UserResponseDTO(
                        u.UserId,
                        u.FirstName,
                        u.LastName,
                        u.Subscription
                    )
                )
            );

            if (firstname != null)
            {
                resultList = resultList.Where(f =>
                {
                    try
                    {
                        return f.FirstName == firstname;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"No user with the firstname {firstname} has been found.");
                    }
                }).ToList();
            }
            if (lastname != null)
            {
                resultList = resultList.Where(l =>
                {
                    try
                    {
                        return l.LastName == lastname;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"No user with the firstname {lastname} has been found.");
                    }
                }).ToList();
            }
            if (subcribe != null)
            {
                resultList = resultList.Where(s =>
                {
                    try
                    {
                        return s.Subscription == bool.Parse(subcribe);
                    }
                    catch
                    {
                        throw new InvalidOperationException("Please either use true or false");
                    }
                }).ToList();
            }

            return resultList.Count != 0 ? resultList : new List<UserResponseDTO>();
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new NullReferenceException($"{nameof(userDTO)} cannot be null.");
            }

            if (await GetUserByEmail(userDTO.Email) == null)
            {
                User user = new User()
                {
                    UserId = Guid.NewGuid(),
                    FirstName = !string.IsNullOrEmpty(userDTO.FirstName) ? userDTO.FirstName : throw new ArgumentNullException($"Invalid {nameof(userDTO.FirstName)} provided"),
                    LastName = !string.IsNullOrEmpty(userDTO.LastName) ? userDTO.LastName : throw new ArgumentNullException($"Invalid {nameof(userDTO.LastName)} provided"),
                    Email = IsValidEmail(userDTO.Email),
                    //Email = !string.IsNullOrEmpty(userDTO.Email) ? userDTO.Email : throw new ArgumentNullException($"Invalid {nameof(userDTO.Email)} provided"),
                    Password = ValidateEncryptPassword(userDTO.Password),
                    Subscription = !string.IsNullOrEmpty(userDTO.Subscription.ToString()) ? bool.Parse(userDTO.Subscription.ToString()) : throw new ArgumentNullException($"Invalid {nameof(userDTO.Subscription)} provided"),
                    Role = Role.User,
                    PartitionKey = userDTO.Subscription.ToString()
                };

                return await _userWriteRepository.AddAsync(user);
            }
            else
            {
                throw new InvalidOperationException($"The user email {userDTO.Email} already exists");
            }
        }

        public async Task<User> UpdateUserRoleToAdmin(string userId)
        {
            var id = !string.IsNullOrEmpty(userId) ? userId : throw new ArgumentNullException($"{userId} cannot be null or empty string.");
            User userData = await GetUserById(userId, true);
            if (userData != null)
            {
                //update user to be an admin
                userData.Role = Role.Admin;

                return await _userWriteRepository.Update(userData);
            }
            throw new InvalidOperationException($"The user ID {userId} provided is invalid.");

        }

        public async Task<User> UpdateUser(UserDTO userDTO, string userId)
        {
            if (userDTO == null)
            {
                throw new NullReferenceException($"{nameof(userDTO)} cannot be null.");
            }

            User userData = await GetUserById(userId, true);
            if (userData != null)
            {
                //update user info
                userData.FirstName = userDTO.FirstName;
                userData.LastName = userDTO.LastName;
                userData.Email = userDTO.Email;
                userData.Password = ValidateEncryptPassword(userDTO.Password);
                userData.Subscription = bool.Parse(userDTO.Subscription.ToString());

                return await _userWriteRepository.Update(userData);
            }
            throw new InvalidOperationException("Please check your User ID again");

        }

        public async Task DeleteUserAsync(string userId)
        {
            var id = !string.IsNullOrEmpty(userId) ? userId : throw new ArgumentNullException($"{userId} cannot be null or empty string.");
            User user = await GetUserById(id, true);

            if (user != null)
            {
                await _userWriteRepository.Delete(user);
            }
            else
            {
                throw new InvalidOperationException($"The user ID {userId} provided is invalid.");
            }
        }

        public string ValidateEncryptPassword(string password)
        {
            string patternPassword = @"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,20}$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    throw new InvalidOperationException(" Password must be at least 8 characters, no more than 20 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit.");
                }
            }
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string IsValidEmail(string email)
        {
            string patternEmail = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!string.IsNullOrEmpty(email))
            {
                if (!Regex.IsMatch(email, patternEmail))
                {
                    throw new InvalidOperationException("Invalid email");
                }
            }
            return patternEmail;
        }
    }
        
    
}

