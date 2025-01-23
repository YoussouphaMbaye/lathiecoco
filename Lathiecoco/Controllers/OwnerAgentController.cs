using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerAgentController : ControllerBase
    {
        private readonly CatalogDbContext _catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AgentOwnerRep _agentOwnerServ;
        private readonly IHttpContextAccessor _contextAccessor;
        public OwnerAgentController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AgentOwnerRep agentOwnerServ,
            IHttpContextAccessor contextAccessor)
        {
            _catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _agentOwnerServ = agentOwnerServ;
            _contextAccessor = contextAccessor;
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
        [HttpPut("/agentOwner")]
        public async Task<ActionResult> updateAgentOwner([FromBody] BodyAgentOwnerUpdateDto oa,Ulid idOwnerAgent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agentOwnerServ.updateOwnerAgent(oa, idOwnerAgent);
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


            string mtoken = "";
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent user = await _catalogDbContext.OwnerAgents.Where(s => s.Login == ldto.username).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account is blocked";
                        rp.Code = 322;
                        return Ok(rp);
                    }
                    if (!user.IsActive)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account not active";
                        rp.Code = 320;
                        return Ok(rp);
                    }
                    //if (BCrypt.Net.BCrypt.EnhancedVerify(us.password, user.Password))
                    if (ldto.password == user.Password)
                    {
                        //var token = await BuildToken(userInfo, new[] { RoleTypes.User });
                        var claims = new List<Claim>() {
                          new Claim("Username", user.Login),
                          new Claim("id",user.IdOwnerAgent.ToString()),
                          new Claim(ClaimTypes.Role, user.Profil)

                        };


                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var expiration = DateTime.UtcNow.AddMinutes(5);
                        JwtSecurityToken token = new JwtSecurityToken(
                           issuer: null,
                           audience: null,
                           claims: claims,
                           expires: expiration,
                           signingCredentials: creds
                        );

                        Console.WriteLine("====================tttt======================");
                        mtoken = new JwtSecurityTokenHandler().WriteToken(token);
                        Console.WriteLine(mtoken);

                        if (user.LoginCount > 1)
                        {
                            user.LoginCount = 0;
                            _catalogDbContext.OwnerAgents.Update(user);
                            await _catalogDbContext.SaveChangesAsync();
                        }

                        _catalogDbContext.OwnerAgents.Update(user);
                        await _catalogDbContext.SaveChangesAsync();

                        _contextAccessor.HttpContext.Response.Cookies.Append("token", mtoken);
                        rp.Body = user;

                    }
                    else
                    {
                        user.LoginCount = user.LoginCount + 1;
                        _catalogDbContext.OwnerAgents.Update(user);
                        await _catalogDbContext.SaveChangesAsync();
                        if (user.LoginCount > 5)
                        {
                            user.IsBlocked = true;
                            _catalogDbContext.OwnerAgents.Update(user);
                            await _catalogDbContext.SaveChangesAsync();
                        }
                        rp.Msg = "Login or password incorrect";
                        rp.IsError = true;
                        return Ok(rp);

                        //return BadRequest();

                    }

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Login or password incorrect";
                    rp.Code = 330;
                    return Ok(rp);
                    //throw new Exception("Login or password incorrect");
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Body = null;

            }
            return Ok(rp);

            //var rep = await _agentOwnerServ.login(ldto);
            //return Ok(rep);


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
