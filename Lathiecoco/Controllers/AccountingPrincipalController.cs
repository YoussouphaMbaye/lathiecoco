using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingPrincipalController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AccountingPrincipalRep _accountingPrincipalServ;
        public AccountingPrincipalController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AccountingPrincipalRep accountingPrincipalServ)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _accountingPrincipalServ = accountingPrincipalServ;
        }
        [HttpPost("/accountingPrincipal")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<AccountingPrincipal>> PostAccountingPrincipal([FromBody] AccountingPrincipal ac)
        {

            return await _accountingPrincipalServ.addAgAccountingPrincipal(ac);

        }
        [HttpGet("/accountingPrincipal/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AccountingPrincipal>>> findAllAccountingPrincipal(int page = 1, int limit = 10)
        {

            return await _accountingPrincipalServ.findAllAgAccountingPrincipal(page, limit);

        }
    }
}
