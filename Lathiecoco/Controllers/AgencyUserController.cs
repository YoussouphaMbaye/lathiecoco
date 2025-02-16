using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        [HttpPost("/agency-user")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> PostAgentOwner([FromBody] BodyAgencyUserDto oa)
        {
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           var rep= await _agencyUserServ.addAgencyUser(oa);
           return Ok(rep);

        }

        [HttpPost("/agency-user/change-password")]
        public async Task<ActionResult> updatePassword(ChangePasswordDto cp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _agencyUserServ.updatePassword(cp);
            return Ok(res);
        }

        [HttpPut("/agency-user")]
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
        
        [HttpPut("/agency-user/activate-or-deactivate")]
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

        [HttpPut("/agency-user/block-or-deblock")]
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
        [HttpPost("/agency-user/refresh-token")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
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

               AgencyUser user= await  _catalogDbContext.AgencyUsers.FirstOrDefaultAsync(au=>au.TokenRefresh==refreshToken);
               
                if (user==null)
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
                          new Claim("id",user.IdAgencyUser.ToString()),
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
                rp.IsError= true;
                rp.Msg = ex.Message;
                return BadRequest(rp);
            }
           

        }
        [HttpPost("/agency-user/login")]
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

                        mtoken = getToken(claims);

                     if (user.LoginCount > 1)
                        {
                            user.LoginCount = 0;
                            _catalogDbContext.AgencyUsers.Update(user);
                            await _catalogDbContext.SaveChangesAsync();
                        }

                        _catalogDbContext.AgencyUsers.Update(user);
                        await _catalogDbContext.SaveChangesAsync();

                    
                        var refreshToken = genrateRefreshToken();
                        await setRefreshToken(refreshToken,user);

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
        public async Task setRefreshToken(string refreshToken,AgencyUser user)
        {
            var newDate = DateTime.UtcNow.AddMinutes(60*24);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newDate,
            };

            _contextAccessor.HttpContext.Response.Cookies.Append("tokenRefresh", refreshToken, cookieOptions);

            user.TokenRefresh= refreshToken;
            user.ExpireDateTokenRefresh = newDate;
            _catalogDbContext.AgencyUsers.Update(user);
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

        [HttpGet("/agency-user/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AgencyUser>>> findAllAgentOwner(int page = 1, int limit = 10)
        {

            return await _agencyUserServ.findAllAgentsUser(page, limit);

        }

        [HttpGet("/agency-user/find-all-by-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AgencyUser>>> findAllByAgency(Ulid IdAgency, int page = 1, int limit = 10)
        {

            return await _agencyUserServ.findAgencyUsertByAgency(IdAgency,page, limit);

        }
        



    }
}
