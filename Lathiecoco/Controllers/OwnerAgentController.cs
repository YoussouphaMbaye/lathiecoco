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
    public class OwnerAgentController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AgentOwnerRep _agentOwnerServ;
        public OwnerAgentController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AgentOwnerRep agentOwnerServ)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _agentOwnerServ = agentOwnerServ;
        }
        [HttpPost("/agentOwner")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> PostAgentOwner([FromBody] BodyOwnerAgentDto oa)
        {
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           var rep= await _agentOwnerServ.addOwnerAgent(oa);
            return Ok(rep);


        }
        [HttpPost("/agentOwner/login")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> LoginAgentOwner([FromBody] LoginDto ldto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agentOwnerServ.login(ldto);
            return Ok(rep);


        }
        [HttpGet("/ownerAgent/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<OwnerAgent>>> findAllAgentOwner(int page = 1, int limit = 10)
        {

            return await _agentOwnerServ.findAllOwnerAgents(page, limit);

        }
        [HttpGet("/ownerAgent/getStatistics")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<BodyNbCountDto>> getStatistics()
        {

            return await _agentOwnerServ.getStatistique();

        }

    }
}
