using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillerInvoiceController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private BilllerInvoiceRep _billerInvoiceServ;
        public BillerInvoiceController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
           BilllerInvoiceRep billerInvoiceServ)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _billerInvoiceServ = billerInvoiceServ;
        }
        [HttpGet("/billerInvoice")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<BillerInvoice>>> findAllbillerInvoice(int page = 1, int limit = 10)
        {

            return await _billerInvoiceServ.findAllBillerInvoice(page,limit);

        }
        [HttpPost("/billerInvoice")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<BillerInvoice>> addbillerInvoice(BodyBillerDto biller)
        {

            return await _billerInvoiceServ.insertBillerInvoice(biller);

        }
        [HttpGet("/billerInvoice/findById")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<BillerInvoice>> billerById(Ulid id)
        {

            return await _billerInvoiceServ.findBillerInvoiceById(id);

        }

    }
}
