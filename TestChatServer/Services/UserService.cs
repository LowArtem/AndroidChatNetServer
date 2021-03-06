using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.Data.Repositories;

namespace TestChatServer.Services
{
    public class UserService
    {
        public const string FORBIDDEN_SYMBOLS = "$@$-%@%";


        private readonly IRepository<User> _userRepo;
        private readonly ILogger logger;

        public UserService(IRepository<User> userRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            this.logger = logger;
        }

        public async Task<User> SaveUser(User user)
        {
            try
            {
                return await _userRepo.AddAsync(user);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateUser(long id, User newUser)
        {
            try
            {
                var oldUser = await _userRepo.GetAsync(id);
                if (oldUser == null)
                {
                    logger.LogWarning("UpdateUser -> cannot get user {0}. It is null", id);
                    return false;
                }

                oldUser.Username = newUser.Username;
                oldUser.Password = newUser.Password;
                oldUser.Icon = newUser.Icon;
                oldUser.IsOnline = newUser.IsOnline;
                oldUser.LastDate = newUser.LastDate;

                await _userRepo.UpdateAsync(oldUser);
                return true;
            }
            catch (Exception e)
            {
                logger.LogWarning("UpdateUser -> error: {0}", e.ToString());
                return false;
            }
        }

        public async Task Delete(User user)
        {
            try
            {
                await _userRepo.RemoveAsync(user.Id);
            }
            catch (Exception)
            {

            }
        }

        public async Task<User> GetUser(long id)
        {
            return await _userRepo.GetAsync(id);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepo.Items.Where(u => u.Username == username).SingleOrDefault();
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _userRepo.Items.Where(u => u.Username == username && u.Password == password).SingleOrDefault();
        }

        public List<User> GetAllUsers()
        {
            return _userRepo.Items.ToList();
        }

        public List<User> FindUserByUsername(string username)
        {
            return _userRepo.Items
                .Where(u => u.Username.Replace(" ", "").ToLower().Contains(username.Replace(" ", "").ToLower()))
                .ToList();
        }
    }
}
