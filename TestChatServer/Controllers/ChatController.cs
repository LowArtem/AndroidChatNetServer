using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.BL;
using TestChatServer.DTO;

namespace TestChatServer.Controllers
{
    [ApiController]
    [Route("api/chats/[action]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatBL chatBL;

        public ChatController(ChatBL chatBL)
        {
            this.chatBL = chatBL;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatInfoDTO>>> GetAllChatsByUser([FromQuery] long userId)
        {
            if (userId < 0) return new BadRequestResult();

            try
            {
                return await chatBL.GetAllChatInfoByUser(userId);
            }
            catch (Exception)
            {
                return new BadRequestResult();
                throw;
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<ActionResult<ChatDetailsDTO>> GetChat(long id)
        {
            if (id < 0) return new BadRequestResult();

            try
            {
                var chat = await chatBL.GetChatById(id);

                if (chat == null) return new BadRequestResult();

                return chat;
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetChatMembers(long chatId)
        {
            var members = await chatBL.GetChatMembers(chatId);
            if (members == null) return new BadRequestResult();

            return members;
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateChat([FromBody] ChatCreatingDTO newChat)
        {
            var chatId = await chatBL.CreateChat(newChat);

            if (chatId == -1) return new BadRequestResult();
            return chatId;
        }

        [HttpGet]
        public async Task<ActionResult<long>> CreateDialog(long creatorId, long secondUserId)
        {
            var chatId = await chatBL.CreateOrGetDialog(creatorId, secondUserId);

            if (chatId == -1) return new BadRequestResult();
            return chatId;
        }


        [HttpPut]
        [Route("{id:long}")]
        public async Task<ActionResult> Update(long id, [FromBody] ChatUpdatingDTO chatUpdating)
        {
            if (await chatBL.UpdateChat(id, chatUpdating)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteChat(long chatId, long currentUserId)
        {
            if (await chatBL.DeleteChat(chatId, currentUserId)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpGet]
        public ActionResult<List<ChatInfoDTO>> FindChats(string name)
        {
            try
            {
                return chatBL.FindChatsByName(name);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddMember(long chatId, long userId)
        {
            if (await chatBL.AddUserToChat(chatId, userId)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMember(long chatId, long userId)
        {
            if (await chatBL.DeleteUserFromChat(chatId, userId)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpGet]
        public async Task<ActionResult<List<long>>> GetAdministratorIds(long chatId)
        {
            var admins = await chatBL.GetChatAdmins(chatId);
            if (admins == null) return new BadRequestResult();

            return admins;
        }

        [HttpPost]
        public async Task<ActionResult> AddAdministrator(long chatId, long userId)
        {
            if (await chatBL.AddAdminToChat(chatId, userId)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAdministrator(long chatId, long adminId)
        {
            if (await chatBL.DeleteAdmin(chatId, adminId)) return new OkResult();
            else return new BadRequestResult();
        }
    }
}
