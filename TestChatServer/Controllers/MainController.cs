using Microsoft.AspNetCore.Mvc;

namespace TestChatServer.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MainController : ControllerBase
    {

        [HttpGet]
        public ActionResult CheckServerStatus()
        {
            return new OkResult();
        }
    }
}
