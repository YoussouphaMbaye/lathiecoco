using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;

using Microsoft.AspNetCore.Mvc;


namespace Lathiecoco.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private readonly AgencyRep _agencyService;
        private readonly CatalogDbContext _CatalogDbContext;
        public AgencyController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            CatalogDbContext CatalogDbContext,
            AgencyRep agencyService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;
            this._CatalogDbContext = CatalogDbContext;

            _configuration = configuration;

            _agencyService = agencyService;
        }
        [HttpPost("/agency")]
        public async Task<ActionResult> postAgent(AgencyDto ag)
        {
            var res = await _agencyService.addAgency(ag);
            return Ok(res);
        }
        [HttpPut("/agency")]
        public async Task<ActionResult> putAgent(AgencyPutDto ag,Ulid idAgency)
        {
            var res = await _agencyService.updateAgency(ag,idAgency);
            return Ok(res);
        }

        [HttpGet("/agencies")]
        public async Task<ActionResult> findAll(int page = 1, int limit = 10)
        {
            var res = await _agencyService.findAgencies(page, limit);
            return Ok(res);
        }
        
        [HttpPost("/agencies/definePercentagePurchase")]
        public async Task<ActionResult> definePercentagePurchase(DefinePercentagePurchaseAgentDto dto)
        {
            var res = await _agencyService.definePercentagePurchase(dto);
            return Ok(res);
        }
        
        [HttpGet("/agencies/AgencyById")]
        public async Task<ActionResult> getAgencyById(Ulid id)
        {
            var res = await _agencyService.getAgencyById(id);
            return Ok(res);
        }

        [HttpGet("/agencies/searche")]
        public async Task<ActionResult> searche(string? email, string? code, string? phone, int page = 1, int limit = 10)
        {
            var res = await _agencyService.agencySearch(email, code, phone, page, limit);
            return Ok(res);
        }

    }
}
