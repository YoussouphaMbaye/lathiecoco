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
using System.Security.Cryptography;
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
        [HttpPost("/agent-owner")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> PostAgentOwner([FromBody] BodyOwnerAgentDto oa)
        {
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           var rep= await _agentOwnerServ.addOwnerAgent(oa);
            return Ok(rep);


        }
        [HttpPut("/agent-owner")]
        public async Task<ActionResult> updateAgentOwner([FromBody] BodyAgentOwnerUpdateDto oa,Ulid idOwnerAgent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rep = await _agentOwnerServ.updateOwnerAgent(oa, idOwnerAgent);
            return Ok(rep);


        }

        [HttpPost("/agent-owner/refresh-token")]
        public async Task<ActionResult> refreshToken()
        {
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {

                string refreshToken = _contextAccessor.HttpContext.Request.Cookies["tokenRefresh"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    rp.Code = 400;
                    rp.IsError = true;
                    rp.Msg = "No refresh Token!";

                    return BadRequest(rp);
                }

                OwnerAgent user = await _catalogDbContext.OwnerAgents.FirstOrDefaultAsync(au => au.TokenRefresh == refreshToken);

                if (user == null)
                {
                    rp.Code = 401;
                    rp.IsError = true;
                    rp.Msg = "Invalid refresh Token!";
                    return Unauthorized(rp);
                }

                else if (user.ExpireDateTokenRefresh < DateTime.UtcNow)
                {
                    rp.Code = 401;
                    rp.IsError = true;
                    rp.Msg = "Token Expired!";
                    return Unauthorized(rp);
                }
                //var token = await BuildToken(userInfo, new[] { RoleTypes.User });
                var claims = new List<Claim>() {
                          new Claim("Username", user.Login),
                          new Claim("id",user.IdOwnerAgent.ToString()),
                          new Claim(ClaimTypes.Role, user.Profil)

                        };

                rp.Msg = "Success";
                var mtoken = getToken(claims);
                var newRefreshToken = genrateRefreshToken();
                await setRefreshToken(newRefreshToken, user);

                _contextAccessor.HttpContext.Response.Cookies.Append("token", mtoken);

                return Ok(rp);
            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                return BadRequest(rp);
            }


        }
        [HttpPost("/agent-owner/login")]
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
                        rp.Code = 322;
                        rp.IsError = true;
                        rp.Msg = "Your account is blocked";
                        return Ok(rp);
                    }
                    if (!user.IsActive)
                    {
                        rp.Code = 320;
                        rp.IsError = true;
                        rp.Msg = "Your account not active";
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

                        var newRefreshToken = genrateRefreshToken();
                        await setRefreshToken(newRefreshToken, user);
                        mtoken = getToken(claims);

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
                    rp.Code = 330;
                    rp.IsError = true;
                    rp.Msg = "Login or password incorrect";
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

        }

        [NonAction]
        public string getToken(List<Claim> claims)
        {
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

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        [NonAction]
        public async Task setRefreshToken(string refreshToken, OwnerAgent user)
        {
            var newDate = DateTime.UtcNow.AddMinutes(60 * 24);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newDate,
            };

            _contextAccessor.HttpContext.Response.Cookies.Append("tokenRefresh", refreshToken, cookieOptions);

            user.TokenRefresh = refreshToken;
            user.ExpireDateTokenRefresh = newDate;
            _catalogDbContext.OwnerAgents.Update(user);
            await _catalogDbContext.SaveChangesAsync();


        }

        [NonAction]
        public string genrateRefreshToken()

        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }

        [HttpGet("/owner-agent/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<OwnerAgent>>> findAllAgentOwner(int page = 1, int limit = 10)
        {

            return await _agentOwnerServ.findAllOwnerAgents(page, limit);

        }
        [HttpGet("/owner-agent/statistics")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<BodyNbCountDto>> getStatistics()
        {

            return await _agentOwnerServ.getStatistique();

        }

    }
}
