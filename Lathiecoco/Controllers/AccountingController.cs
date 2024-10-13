using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Mvc;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AccountingRep _accountingRepService;
        public AccountingController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AccountingRep accountingService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _accountingRepService = accountingService;
        }
        [HttpPost("/accounting")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<Accounting>> PostAccounting([FromBody] Accounting ac)
        {

            return await _accountingRepService.addAccounting(ac);

        }
        [HttpGet("/accounting/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<Accounting>>> findAllAccounting(int page = 1, int limit = 10)
        {

            return await _accountingRepService.findAllAccounting(page, limit);

        }
    }
}
