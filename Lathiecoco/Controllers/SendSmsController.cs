using Lathiecoco.models;
using Lathiecoco.repository.SMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendSmsController : ControllerBase
    {
        private readonly SmsSendRep _smsSendRep;
        public SendSmsController(SmsSendRep smsSendRep)
        {
            _smsSendRep= smsSendRep;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SendSms(string phoneNumber, string message)
        {
            ResponseBody<string> rp = await _smsSendRep.sendSms(phoneNumber, message);
            return Ok(rp);
        }


    }
}
