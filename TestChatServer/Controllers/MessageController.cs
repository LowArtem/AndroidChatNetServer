using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.BL;
using TestChatServer.DTO;

namespace TestChatServer.Controllers
{
    [ApiController]
    [Route("api/messages/[action]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageBL messageBL;

        public MessageController(MessageBL messageBL)
        {
            this.messageBL = messageBL;
        }

        [HttpPost]
        [Route("{chatId:long}")]
        public async Task<ActionResult> SaveMessage(long chatId, [FromBody] MessageWithUserAuthDTO messageDTO)
        {
            if (await messageBL.Save(chatId, messageDTO)) return new OkResult();
            else return new BadRequestResult();
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageDTO>>> GetAllMessages(long chatId)
        {
            var messages = await messageBL.GetAllMessagesByChat(chatId);
            if (messages == null || messages.Count == 0)
            {
                return new List<MessageDTO>();
            }
            else
            {
                return messages;
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageDTO>>> GetMessagesPage(long chatId, int messagesCount, int startIndex)
        {
            try
            {
                return await messageBL.GetPageOfMessages(chatId, messagesCount, startIndex);
            }
            catch (System.Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        public ActionResult<List<MessageDTO>> GetMessagesByUser([FromQuery] UserMessageWithoutIconDTO userDTO)
        {
            var messages = messageBL.GetAllMessagesByUser(userDTO.Id);
            if (messages == null || messages.Count == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return messages;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMessage(long chatId, long messageId)
        {
            if (await messageBL.DeleteMessageFromChat(chatId, messageId)) return new OkResult();
            else return new BadRequestResult();
        }
    }
}
