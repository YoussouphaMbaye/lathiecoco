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
    public class AgencyUserController : ControllerBase
    {
        private readonly CatalogDbContext _catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AgencyUserRep _agencyUserServ;
        private readonly IHttpContextAccessor _contextAccessor;
        public AgencyUserController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AgencyUserRep agencyUserServ,
            IHttpContextAccessor contextAccessor)
        {
            _catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _agencyUserServ = agencyUserServ;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("/agencyUser")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> PostAgentOwner([FromBody] BodyAgencyUserDto oa)
        {
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           var rep= await _agencyUserServ.addAgencyUser(oa);
           return Ok(rep);

        }

        [HttpPut("/agencyUser")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> PutAgentOwner([FromBody] BodyAgencyUserUpdateDto oa, Ulid idAgencyUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agencyUserServ.updateAgencyUser(oa, idAgencyUser);
            return Ok(rep);

        }
        
        [HttpPut("/agencyUser/activateOrDeactive")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> activateOrDeactive(ActiveBlockDto oa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agencyUserServ.activateOrDeactiveAgencyUser(oa);
            return Ok(rep);

        }

        [HttpPut("/agencyUser/blockOrDeblock")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> blockOrDeblock(ActiveBlockDto oa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agencyUserServ.blockOrDeblockAgencyUser(oa);
            return Ok(rep);

        }
        [HttpPost("/agencyUser/login")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> LoginAgentOwner([FromBody] LoginDto ldto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            string mtoken = "";
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser user = await _catalogDbContext.AgencyUsers.Where(s => s.Login == ldto.username).FirstOrDefaultAsync();
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
                          new Claim("id",user.IdAgencyUser.ToString()),
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
                            _catalogDbContext.AgencyUsers.Update(user);
                            await _catalogDbContext.SaveChangesAsync();
                        }

                        _catalogDbContext.AgencyUsers.Update(user);
                        await _catalogDbContext.SaveChangesAsync();

                        _contextAccessor.HttpContext.Response.Cookies.Append("token", mtoken);
                        rp.Body = user;

                    }
                    else
                    {
                        user.LoginCount = user.LoginCount + 1;
                        _catalogDbContext.AgencyUsers.Update(user);
                        await _catalogDbContext.SaveChangesAsync();
                        if (user.LoginCount > 5)
                        {
                            user.IsBlocked = true;
                            _catalogDbContext.AgencyUsers.Update(user);
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

            //var rep = await _agencyUserServ.login(ldto);
            //return Ok(rep);


        }
        [HttpGet("/agencyUser/findAll")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AgencyUser>>> findAllAgentOwner(int page = 1, int limit = 10)
        {

            return await _agencyUserServ.findAllAgentsUser(page, limit);

        }

        [HttpGet("/agencyUser/findAllByAgency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AgencyUser>>> findAllByAgency(Ulid IdAgency, int page = 1, int limit = 10)
        {

            return await _agencyUserServ.findAgencyUsertByAgency(IdAgency,page, limit);

        }
        



    }
}
