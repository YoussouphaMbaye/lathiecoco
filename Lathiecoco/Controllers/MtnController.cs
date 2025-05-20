
using Lathiecoco.models.conlog;
using Lathiecoco.models;
using Lathiecoco.repository.Conlog;
using Lathiecoco.repository.Orange;
using Microsoft.AspNetCore.Mvc;
using Lathiecoco.models.orange;
using Lathiecoco.models.notifications;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Lathiecoco.repository.Mtn;
using Lathiecoco.models.mtn;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtnController : ControllerBase
    {
        private readonly MtnRep _mtnRep;
        private readonly paymentNotificationsRep _notifications;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environnement;
        public MtnController(MtnRep mtnRep , paymentNotificationsRep notifications, IConfiguration configuration, IWebHostEnvironment environment ) {
            _mtnRep = mtnRep;
            _notifications =notifications;
            _configuration = configuration;
            _environnement = environment;
        }

        [HttpPost("/requestToPay")]
        public async Task<ResponseBody<string>> payTransaction([FromBody] mtnPaymentRequest requestToPay)
        {

            return await _mtnRep.MtnTransactionProcess(requestToPay);

        }

        [HttpPost("/mtn-otifications")]
        public async Task<ResponseBody<string>> orangeNotification([FromBody] Notifications notification, [FromHeader] string? Authorization)
        {
            var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var AuthenticationToken = builder.Build().GetSection("Authentication").GetSection("BasicAuth").Value;
            if ((AuthenticationToken != Authorization) || ( Authorization == null ))
            {
                HttpContext.Response.StatusCode = 401;
                ResponseBody<string> rp = new ResponseBody<string>();
                rp.Code = 401;
                rp.IsError = true;
                rp.Msg = "Wrong credentials";
             
                return rp;
            }
            


            return await _notifications.orangeMoneyNotificationsHandler(notification);

        }
    }
}
