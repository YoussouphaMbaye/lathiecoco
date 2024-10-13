using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceStartupController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private InvoiceStartupMasterRep _invoiceStartupMasterServ;
        public InvoiceStartupController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,
       IConfiguration configuration,
            InvoiceStartupMasterRep invoiceStartupMasterServ)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            this._configuration = configuration;

            this._invoiceStartupMasterServ = invoiceStartupMasterServ;
        }
        [HttpPost("/invoiceStartupMaster/initiateInvoiceStartupMasterServ")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> intiateMasterAgencyTransactions([FromBody] BodyInvoiceStartupMaster ma)
        {
            var res = await _invoiceStartupMasterServ.initiateInvoiceStarupMaster(ma);
            return Ok(res);
        }
        [HttpGet("/invoiceStartupMaster/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> findAll(int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findAllInvoiceStartupMasterAgency(page, limit);
            return Ok(res);
        }
        [HttpPost("/invoiceStartupMaster/validateTransactions")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> validateTransactions(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster)
        {
            var res = await _invoiceStartupMasterServ.ValidateInvoiceStartupMaster(bodyValidInvoiceStartupMaster);
            return Ok(res);
        }
        [HttpGet("/invoiceStartupMaster/byStaff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byStaff(Ulid fkIdOwnerAgent,int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStartupByOwnerAgent(fkIdOwnerAgent,page,limit);
            return Ok(res);
        }
        [HttpGet("/invoiceStartupMaster/byId")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> byId(Ulid id)
        {
            var res = await _invoiceStartupMasterServ.initiateInvoiceStarupMasterById(id);
            return Ok(res);
        }
        [HttpGet("/invoiceStartupMaster/byMaster")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byMaster(Ulid fkIdMaster, int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStartupByMaster(fkIdMaster, page, limit);
            return Ok(res);
        }
    }
}
