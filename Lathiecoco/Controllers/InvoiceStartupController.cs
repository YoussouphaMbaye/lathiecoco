using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpPost("/invoice-startup-master/initiate-invoice-startup-master")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> intiateMasterAgencyTransactions([FromBody] BodyInvoiceStartupMaster ma)
        {
            var res = await _invoiceStartupMasterServ.initiateInvoiceStarupMaster(ma);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("/invoice-startup-master/initiate-invoice-startup-master-by-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> initiateInvoiceStarupMasterByAgency([FromBody] BodyInvoiceStartupMasterByAgent body)
        {
            var res = await _invoiceStartupMasterServ.initiateInvoiceStarupMasterByAgency(body);
            return Ok(res);
        }

        [HttpPost("/UploadFile")]
        public async Task<ActionResult> UploadFile(IFormFile formFile,Ulid idInvoiceStartupMaster)
        {

            var res = await _invoiceStartupMasterServ.uploadProof(formFile, idInvoiceStartupMaster);


            return Ok(res);
        }
        [HttpGet("/getFileUploaded")]
        public async Task<ActionResult> getFileUploaded(string key)
        {

            var res = await _invoiceStartupMasterServ.getFileUrl(key);


            return Ok(res);
        }
        [Authorize]
        [HttpGet("/invoice-startup-master/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> findAll(int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findAllInvoiceStartupMasterAgency(page, limit);
            return Ok(res);
        }

        [Authorize(Roles = "COMPTABLE,SUPADMIN")]
        [HttpPost("/invoice-startup-master/validate-transactions")]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> validateTransactions(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster)
        {
            var res = await _invoiceStartupMasterServ.ValidateInvoiceStartupMaster(bodyValidInvoiceStartupMaster);
            return Ok(res);
        }

        [Authorize(Roles = "COMPTABLE,SUPADMIN")]
        [HttpPost("/invoice-startup-master/validate-invoice-startup-master-of-agency-by-staff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> validateInvoiceStartupMasterOfAgency(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster)
        {
            var res = await _invoiceStartupMasterServ.validateInvoiceStartupMasterOfAgency(bodyValidInvoiceStartupMaster);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("/invoice-startup-master/validate-transactions-by-agency-user")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> validateTransactionByAgencyUser(ValidateInvoiceStartupMasterDto dto)
        {
            var res = await _invoiceStartupMasterServ.ValidateInvoiceStartupMasterByAgencyUser(dto);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/by-staff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byStaff(Ulid fkIdOwnerAgent,int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStartupByOwnerAgent(fkIdOwnerAgent,page,limit);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/by-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byAgency(Ulid idAgency, int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStartupByAgency(idAgency, page, limit);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/by-agency/search")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byAgencySearch(string? status, string? code, DateTime? beginDate, DateTime? endDate, String agenceCode, String? paymentMethod, int page=1, int limit=10)
        {
            var res = await _invoiceStartupMasterServ.searcheInvoiceStartupMasterByAgency(status,code,beginDate,endDate,agenceCode, paymentMethod, page, limit);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/by-id")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<ResponseBody<InvoiceStartupMaster>>> byId(Ulid id)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStarupMasterById(id);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/by-master")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult<List<ResponseBody<InvoiceStartupMaster>>>> byMaster(Ulid fkIdMaster, int page = 1, int limit = 10)
        {
            var res = await _invoiceStartupMasterServ.findInvoiceStartupByMaster(fkIdMaster, page, limit);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/invoice-startup-master/search")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<InvoiceStartupMaster>>> searcheInvoiceStartupMaster(string? status, string? code, DateTime? beginDate, DateTime? endDate, String? agenceCode, String? staffEmail, int page = 1, int limit = 10)
        {
            return await _invoiceStartupMasterServ.searcheInvoiceStartupMaster(status, code, beginDate, endDate, agenceCode, staffEmail, page, limit)
;
        }
    }
}
