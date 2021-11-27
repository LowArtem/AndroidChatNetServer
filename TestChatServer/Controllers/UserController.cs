using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.BL;
using TestChatServer.Data.Entity;
using TestChatServer.DTO;

namespace TestChatServer.Controllers
{
    [ApiController]
    [Route("api/users/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserBL userBL;

        public UserController(UserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpGet]
        public ActionResult<UserDTO> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = userBL.GetUserByUsernameAndPassword(username, password);
            if (user == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return user;
            }
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetUserIsOnline([FromQuery] long id)
        {
            return await userBL.GetUserIsOnline(id);
        }

        [HttpGet]
        public async Task<ActionResult<DateTime>> GetUserLastDate([FromQuery] long id)
        {
            try
            {
                return await userBL.GetUserLastDate(id);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        public ActionResult<List<UserDTO>> FindUserByUsername([FromQuery] string username)
        {
            try
            {
                return userBL.FindUserByUsername(username);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPut]
        [Route("{id:long}")]
        public async Task<ActionResult> UpdateUser(long id, [FromBody] User user)
        {
            bool isUpdated = await userBL.Update(id, user);
            if (isUpdated) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpPost]
        public async Task<ActionResult<User>> SaveUser([FromBody] User user)
        {
            var saved = await userBL.Save(user);

            if (saved != null) return saved;
            else return new BadRequestResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromBody] UserWithoutIconDTO user)
        {
            if (await userBL.Delete(user)) return new OkResult();
            else return new BadRequestResult();
        }
    }
}
