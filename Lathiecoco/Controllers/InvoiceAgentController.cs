using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceAgentController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private InvoiceAgentRep _invoiceWalletCashierService;
        public InvoiceAgentController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            InvoiceAgentRep invoiceWalletCashService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _invoiceWalletCashierService = invoiceWalletCashService;
        }
        [HttpPost("/invoiceWalletAgent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> PostInvoiceWalletCashier([FromBody] InvoiceWalletAgent ac)
        {

            return await _invoiceWalletCashierService.addInvoiceWallet(ac);

        }
        [HttpPost("/invoiceWalletAgent/withdraw")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> withdraw([FromBody] BodyInvoiceWalletCashier ac)
        {

            return await _invoiceWalletCashierService.withdraw(ac);

        }
        [HttpPost("/invoiceWalletAgent/deposit")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> deposit([FromBody] BodyInvoiceWalletCashier ac)
        {

            return await _invoiceWalletCashierService.deposit(ac);

        }
        [HttpGet("/invoiceWalletAgent/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<InvoiceWalletAgent>>> findAllAccounting(int page = 1, int limit = 10)
        {

            return await _invoiceWalletCashierService.findAllInvoiceWallet(page, limit);

        }
        [HttpGet("/invoiceWalletAgent/findById")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> findById(Ulid id)
        {

            return await _invoiceWalletCashierService.findWalletCashierById(id);

        }

    }
}
