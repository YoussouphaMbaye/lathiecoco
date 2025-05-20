using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace  Lathiecoco.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentModeController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private PaymentModeRep _paymentModeServ;
        public PaymentModeController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            PaymentModeRep paymentModeServ)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _paymentModeServ = paymentModeServ;
        }

        [Authorize(Roles = "SUPADMIN")]
        [HttpPost("/paymentMode")]
        
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult< ResponseBody<PaymentMode>>> addPaymentMode([FromBody] PaymentMode py)
        {
            var res = await _paymentModeServ.addPaymentMode(py);
            return Ok(res);

        }

        [HttpGet("/paymentMode/test")]
        public IEnumerable<String> tester()
        {
            return new List<String>() { "1", "2", "3" };
        }

        [Authorize(Roles = "SUPADMIN")]
        [HttpGet("/paymentMode/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<List<PaymentMode>>>> findAllAccounting(int page = 1, int limit = 10)
        {
            

             var res=await _paymentModeServ.findAllPaymentMode(page, limit);
             return Ok(res);

        }

        [Authorize(Roles = "SUPADMIN")]
        [HttpPut("/paymentMode/update")]
        public async Task<ActionResult> updatePaymentMode(PaymentModeDto pm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _paymentModeServ.updatePaymentMode(pm);
            return Ok(res);
        }

        [Authorize(Roles = "SUPADMIN")]
        [HttpPut("/paymentMode/activate-or-deactivate")]
        public async Task<ActionResult> activateOrDeactivatePaymentMode(ActiveDeactivePaymentModeDto acPm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _paymentModeServ.activateOrDeactivatePaymentMode(acPm);
            return Ok(res);
        }
    }
}
