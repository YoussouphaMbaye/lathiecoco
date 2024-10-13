using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceWalletController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private InvoiceWalletRep _invoiceWalletRepService;
        public InvoiceWalletController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            InvoiceWalletRep invoiceWalletService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _invoiceWalletRepService = invoiceWalletService;
        }
        [HttpPost("/invoiceWallet")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWallet>> PostInvoiceWallet([FromBody] InvoiceWallet ac)
        {

            return await _invoiceWalletRepService.addInvoiceWallet(ac);

        }
        [HttpPost("/invoiceWallet/insererTransaction")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWallet>> InsererTransaction([FromBody] BodyPostInvoiceWallet ac)
        {

            return await _invoiceWalletRepService.insertInvoiceWallet(ac);

        }
        [HttpGet("/invoiceWallet/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<InvoiceWallet>>> findAllAccounting(int page = 1, int limit = 10)
        {

            return await _invoiceWalletRepService.findAllInvoiceWallet(page, limit);

        }
        [HttpGet("/invoiceWallet/findById")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<InvoiceWallet>> findById(Ulid id)
        {

            return await _invoiceWalletRepService.invoiceWalletWithId(id);

        }
    }
}
