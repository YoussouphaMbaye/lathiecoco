
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
       
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environnement;
        public MtnController(MtnRep mtnRep , IConfiguration configuration, IWebHostEnvironment environment ) {
            _mtnRep = mtnRep;
            _configuration = configuration;
            _environnement = environment;
        }

        [HttpPost("/requestToPay")]
        public async Task<ResponseBody<string>> payTransaction([FromBody] mtnPaymentRequest requestToPay)
        {

            return await _mtnRep.MtnTransactionProcess(requestToPay);

        }

        [HttpPost("/mtn-notifications")]
        public async Task<ResponseBody<string>> MtnNotification([FromBody] requestToPay? rp)
        {
            return await _mtnRep.mtnMoneyNotificationsHandler(rp);

        }
    }
}
