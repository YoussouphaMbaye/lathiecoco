using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
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
        [HttpPost("/paymentMode")]
        
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult< ResponseBody<PaymentMode>>> PostCashier([FromBody] PaymentMode py)
        {
            var res = await _paymentModeServ.addPaymentMode(py);
            return Ok(res);

        }
        [HttpGet("/paymentMode/test")]
        public IEnumerable<String> tester()
        {
            return new List<String>() { "1", "2", "3" };
        }
       
        [HttpGet("/paymentMode/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<List<PaymentMode>>>> findAllAccounting(int page = 1, int limit = 10)
        {
            

             var res=await _paymentModeServ.findAllPaymentMode(page, limit);
            return Ok(res);

        }
    }
}
