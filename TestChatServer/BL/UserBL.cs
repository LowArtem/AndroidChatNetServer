using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.DTO;
using TestChatServer.Services;

namespace TestChatServer.BL
{
    public class UserBL
    {
        private readonly UserService userService;
        private readonly ILogger logger;

        public UserBL(UserService userService, ILogger<UserBL> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<User> Save(User user)
        {
            User checkUser = userService.GetUserByUsername(user.Username);
            if (checkUser != null) return null;

            if (user.Username.Contains(UserService.FORBIDDEN_SYMBOLS)) return null;

            User savingUser = new User(user.Icon, user.Username, user.Password, user.IsOnline, user.LastDate);

            return await userService.SaveUser(savingUser);
        }

        public async Task<bool> Update(long id, User user)
        {
            var nameTestUser = userService.GetUserByUsername(user.Username);
            if (nameTestUser.Id != id)
            {
                logger.LogWarning($"Names are equal:\nid{nameTestUser.Id} - {nameTestUser.Username}\nid{id} - {user.Username}");
                return false;
            }

            return await userService.UpdateUser(id, user);
        }

        public async Task<bool> Delete(UserWithoutIconDTO user)
        {
            User checkUser = await userService.GetUser(user.Id);

            if (checkUser.Password != user.Password || checkUser.Username != user.Username) return false;

            await userService.Delete(checkUser);
            return true;
        }

        public UserDTO GetUserByUsernameAndPassword(string username, string password)
        {
            User user = userService.GetUserByUsernameAndPassword(username, password);
            if (user == null) return null;

            return new UserDTO(user);
        }

        public async Task<DateTime> GetUserLastDate(long id)
        {
            User user = await userService.GetUser(id);
            return user.LastDate;
        }

        public async Task<bool> GetUserIsOnline(long id)
        {
            User user = await userService.GetUser(id);
            if (user == null) return false;

            return user.IsOnline;
        }

        public List<UserDTO> FindUserByUsername(string username)
        {
            if (username.Trim() == "") return new List<UserDTO>();

            var users = userService.FindUserByUsername(username);

            List<UserDTO> usersDTO = new List<UserDTO>();
            foreach (var user in users)
            {
                usersDTO.Add(new UserDTO(user));
            }

            return usersDTO;
        }
    }
}
