﻿
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

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrangeController : ControllerBase
    {
        private readonly OrangeRep _orangeRep;
        private readonly paymentNotificationsRep _notifications;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environnement;
        public OrangeController(OrangeRep orangeRep , paymentNotificationsRep notifications, IConfiguration configuration, IWebHostEnvironment environment ) {
            _orangeRep = orangeRep;
            _notifications =notifications;
            _configuration = configuration;
            _environnement = environment;
        }

        [HttpPost("/transactions")]
        public async Task<ResponseBody<string>> payTransaction([FromBody] TransactionsOrange transaction)
        {

            return await _orangeRep.transactionsProcess(transaction);

        }

        [HttpPost("/notifications")]
        public async Task<ResponseBody<string>> orangeNotification([FromBody] Notifications notification, [FromHeader] string Authorization)
        {
            var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var AuthenticationToken = builder.Build().GetSection("Authentication").GetSection("BasicAuth").Value;
            if (AuthenticationToken != Authorization)
            {
                HttpContext.Response.StatusCode = 401;
                ResponseBody<string> rp = new ResponseBody<string>();
                rp.Code = 401;
                rp.IsError = true;
                rp.Msg = "FAILED";
             
                return rp;
            }


            return await _notifications.orangeMoneyNotificationsHandler(notification);

        }
    }
}
