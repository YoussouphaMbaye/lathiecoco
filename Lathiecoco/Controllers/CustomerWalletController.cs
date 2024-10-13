using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public CustomerWalletController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            CustomerWalletRep custonerWalletService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _custonerWalletService = custonerWalletService;
        }
       
        [HttpPost("/customerWallet/PostCustomerWalletPhoneOnly")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> PostCustomerWalletPhoneOnly([FromBody] BodyCustomerPhoneDto cu)
        {

            return await _custonerWalletService.addCustomerWithAccountingPhoneOnly(cu);

        }
        [HttpPost("/customerWallet/UpdateCustomerInfos")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> UpdateCustomerInfos([FromBody] CustomerUpdateInfosDto cu)
        {

            return await _custonerWalletService.updateCustomerInformations(cu);

        }
        [HttpPost("/customerWallet/UpdateCustomerPin")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> UpdateCustomerPin([FromBody] CustomerUpadatePinDto cu)
        {

            return await _custonerWalletService.updateCustomerPin(cu);

        }
        [HttpPost("/customerWallet/login")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletLogin([FromBody] BodyLoginDto cu)
        {

            return await _custonerWalletService.findCustomerWalletPinContryidentityPhone(cu);

        }
        [HttpPost("/customerWallet/confPinTemp")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> confPinTemp([FromBody] BodyConfPinTempDto cu)
        {

            return await _custonerWalletService.findCustomerWalletPintempContryidentityPhone(cu);

        }
        [HttpPost("/customerWallet/customerWalletWithPhoneAndIdentity")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletWithPhoneAndIdentity([FromBody] BodyPhoneShDto cu)
        {

            return await _custonerWalletService.findCustomerWalletContryidentityAndPhone(cu);

        }
        [HttpPut("/customerWallet/updateByStaff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletUpdateByStaff([FromBody] CustomerUpdateDto cu)
        {

            return await _custonerWalletService.updateCustomerInformationsWithoutPinNumber(cu);

        }
        
        [HttpGet("/customerWallet/findById")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> findById(Ulid id)
        {

            return await _custonerWalletService.findCustomerWalletById(id);

        }
        [HttpGet("/customerWallet/activeOrDesactive")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> activeCustomerWallet(Ulid id)
        {

            return await _custonerWalletService.activateWallet(id);

        }
        [HttpGet("/customerWallet/blockeOrDebloke")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> blockeOrBlocked(Ulid id)
        {

            return await _custonerWalletService.blokeOrDeblokeWallet(id);

        }
        [HttpGet("/customerWallet/customerWithCode")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWithCode(string code)
        {

            return await _custonerWalletService.findCustomerWalletCode(code);

        }
        [HttpGet("/customerWallet/findAllCustomers")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("CUSTOMER",page, limit);

        }
        [HttpGet("/customerWallet/findAllAgents")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgents(int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("AGENT", page, limit);

        }


    }
}
