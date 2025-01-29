using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lathiecoco.services;

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
        [HttpPost("/invoice-wallet-agent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> PostInvoiceWalletCashier([FromBody] InvoiceWalletAgent ac)
        {

            return await _invoiceWalletCashierService.addInvoiceWallet(ac);

        }
        [HttpPost("/invoice-wallet-agent/withdraw")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> withdraw([FromBody] BodyInvoiceWalletCashier ac)
        {

            return await _invoiceWalletCashierService.withdraw(ac);

        }
        [HttpPost("/invoice-wallet-agent/deposit")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> deposit([FromBody] BodyInvoiceWalletCashier ac)
        {

            return await _invoiceWalletCashierService.deposit(ac);

        }
        [HttpGet("/invoice-wallet-agent/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<InvoiceWalletAgent>>> findAllAccounting(int page = 1, int limit = 10)
        {

            return await _invoiceWalletCashierService.findAllInvoiceWallet(page, limit);

        }
        [HttpGet("/invoice-wallet-agent/searche")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<InvoiceWalletAgent>>> searcheinvoiceWalletAgent(string? status, string? code, DateTime? beginDate, DateTime? endDate, int page = 1, int limit = 10)
        {
            return await _invoiceWalletCashierService.searcheInvoiceWalletAgent(status, code, beginDate, endDate, page, limit)
;
        }
        [HttpGet("/invoice-wallet-agent/find-by-id")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWalletAgent>> findById(Ulid id)
        {

            return await _invoiceWalletCashierService.findWalletCashierById(id);

        }
        [HttpGet("/invoice-wallet-agent/deposit-statistic-by-agent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<DepositStatisticByAgentDto>>> depositStatisticByAgentDto(DateTime begenDate, DateTime endDate,string status, Ulid? idAgent)
        {

            return await _invoiceWalletCashierService.depositStatisticByAgentDto(begenDate,endDate, status,idAgent);

        }
        



    }
}
