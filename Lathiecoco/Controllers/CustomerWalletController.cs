using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerWalletController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private CustomerWalletRep _custonerWalletService;

        private readonly IHttpContextAccessor _contextAccessor;
        public CustomerWalletController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            CustomerWalletRep custonerWalletService,
            IHttpContextAccessor contextAccessor)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;
            _contextAccessor = contextAccessor;

            _custonerWalletService = custonerWalletService;
        }
       
        [HttpPost("/customer-wallet/post-customer-wallet-phone-only")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> PostCustomerWalletPhoneOnly([FromBody] BodyCustomerPhoneDto cu)
        {

            return await _custonerWalletService.addCustomerWithAccountingPhoneOnly(cu);

        }
        [HttpPost("/customer-wallet/update-customer-infos")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> UpdateCustomerInfos([FromBody] CustomerUpdateInfosDto cu)
        {

            return await _custonerWalletService.updateCustomerInformations(cu);

        }
        [HttpPost("/customer-wallet/update-customer-pin")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> UpdateCustomerPin([FromBody] CustomerUpadatePinDto cu)
        {

            return await _custonerWalletService.updateCustomerPin(cu);

        }
        [Authorize(Roles = "SUPADMIN")]
        [HttpPost("/customer-wallet/update-pin-by-staff")]
        public async Task<ResponseBody<String>> updateCustomerPinNumberByStaff(UpdateCustomerByStaffDto cus)
        {
            return await _custonerWalletService.updateCustomerPinNumberByStaff(cus);
        }

        [HttpPost("/customer-wallet/login")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletLogin([FromBody] BodyLoginDto cu)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                if (cu.pinNumber != null)
                {
                    cu.pinNumber = cu.pinNumber.Trim().Replace(" ","");
                }

                if (cu.Phone != null)
                {
                    cu.Phone = cu.Phone.Trim().Replace(" ", "");
                }

                CustomerWallet cus = await catalogDbContext.CustomerWallets.Where(c => c.phoneIdentity == cu.CountryIdentity && c.Phone == cu.Phone).FirstOrDefaultAsync();
                if (cus != null)
                {
                    if (!BCrypt.Net.BCrypt.EnhancedVerify(cu.pinNumber,cus.PinNumber))
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or pin not correct";
                        rp.Code = 332;
                        return rp;
                    }

                    if (cus.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Code = 320;
                        rp.Msg = "Your account is blocked!";
                        return rp;
                    }
                    if (!cus.IsActive)
                    {
                        rp.IsError = true;
                        rp.Code = 322;
                        rp.Msg = "Your account is not active!";
                        return rp;
                    }

                    var claims = new List<Claim>() {
                          new Claim("id",cus.IdCustomerWallet.ToString()),
                          new Claim(ClaimTypes.Role, cus.Profile)
                        };

                    string mtoken = getToken(claims);

                    var newDate = DateTime.UtcNow.AddMinutes(60 * 24 * 360 * 1000);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = newDate,
                    };

                    _contextAccessor.HttpContext.Response.Cookies.Append("token", mtoken, cookieOptions);

                    rp.Body = cus;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Phone or pin not correct";
                    rp.Code = 332;
                    return rp;

                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;

            //return await _custonerWalletService.findCustomerWalletPinContryidentityPhone(cu);

        }

        [NonAction]
        public string getToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60*24*360*1000);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost("/customer-wallet/conf-pin-temp")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> confPinTemp([FromBody] BodyConfPinTempDto cu)
        {

            return await _custonerWalletService.findCustomerWalletPintempContryidentityPhone(cu);

        }
        [HttpPost("/customer-wallet/customer-wallet-with-phone-and-identity")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletWithPhoneAndIdentity([FromBody] BodyPhoneShDto cu)
        {

            return await _custonerWalletService.findCustomerWalletContryidentityAndPhone(cu);

        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("/customer-wallet/update-by-staff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletUpdateByStaff([FromBody] CustomerUpdateDto cu)
        {

            return await _custonerWalletService.updateCustomerInformationsWithoutPinNumber(cu);

        }

        
        [HttpGet("/customer-wallet/find-by-id")]
        public async Task<ResponseBody<CustomerWallet>> findById(Ulid id)
        {

            return await _custonerWalletService.findCustomerWalletById(id);

        }

        [Authorize(Roles = "ADMIN,SUPADMIN")]
        [HttpPost("/customer-wallet/active-or-deactivate")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> activeCustomerWallet(ActiveBlockDto dto)
        {

            return await _custonerWalletService.activateWallet(dto);

        }

        [Authorize(Roles = "ADMIN,SUPADMIN")]
        [HttpPost("/customer-wallet/customer-wallet-to-agency")]
        public async Task<ResponseBody<CustomerWallet>> customerWalletToAgency(CustomerToAgencyDto dto)
        {

            return await _custonerWalletService.CustomerToAgencyDto(dto);

        }

        [Authorize(Roles = "ADMIN,SUPADMIN")]
        [HttpPost("/customer-wallet/block-or-deblock")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> blockeOrBlocked(ActiveBlockDto dto)
        {

            return await _custonerWalletService.blokeOrDeblokeWallet(dto);

        }

        [HttpGet("/customer-wallet/customer-with-code")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWithCode(string code)
        {

            return await _custonerWalletService.findCustomerWalletCode(code);

        }

        [Authorize]
        [HttpGet("/customer-wallet/customer-wallet-date-between-and-agent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> getCustomerWalletDateBetweenAndAgent(DateTime begenDate, DateTime endDate, Ulid? idAgent, int page = 1, int limit = 10)

        {

            return await _custonerWalletService.getCustomerWalletDateBetweenAndAgent(begenDate,endDate,idAgent,page,limit);

        }

        [Authorize]
        [HttpGet("/customer-wallet/find-all-customers")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(Ulid? idAgency, String? phone, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("CUSTOMER", idAgency, phone, page, limit);

        }

        [Authorize]
        [HttpPost("/customer-wallet/define-percentage-purchase")]
        public async Task<ActionResult> definePercentagePurchase(DefinePercentagePurchaseMasterDto dto)
        {
            var res = await _custonerWalletService.definePercentagePurchase(dto);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("/customer-wallet/find-all-agents")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgents(Ulid? idAgency,String? phone, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("AGENT", idAgency, phone, page, limit);

        }

        [Authorize]
        [HttpGet("/customer-wallet/find-all-agents-by-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgentsByAgency(Ulid idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllAgentsByAgency(idAgency, page, limit);

        }

    }
}
