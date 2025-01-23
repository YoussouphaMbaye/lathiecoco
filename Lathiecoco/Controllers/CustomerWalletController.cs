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
        [HttpPost("/customerWallet/activeOrDesactive")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> activeCustomerWallet(ActiveBlockDto dto)
        {

            return await _custonerWalletService.activateWallet(dto);

        }
        [HttpPost("/customerWallet/customerWalletToAgency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletToAgency(CustomerToAgencyDto dto)
        {

            return await _custonerWalletService.CustomerToAgencyDto(dto);

        }
        [HttpPost("/customerWallet/blockeOrDebloke")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> blockeOrBlocked(ActiveBlockDto dto)
        {

            return await _custonerWalletService.blokeOrDeblokeWallet(dto);

        }
        [HttpGet("/customerWallet/customerWithCode")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWithCode(string code)
        {

            return await _custonerWalletService.findCustomerWalletCode(code);

        }
        [HttpGet("/customerWallet/getCustomerWalletDateBetweenAndAgent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> getCustomerWalletDateBetweenAndAgent(DateTime begenDate, DateTime endDate, Ulid? idAgent, int page = 1, int limit = 10)

        {

            return await _custonerWalletService.getCustomerWalletDateBetweenAndAgent(begenDate,endDate,idAgent,page,limit);

        }

        [HttpGet("/customerWallet/findAllCustomers")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(Ulid? idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("CUSTOMER", idAgency, page, limit);

        }

        [HttpPost("/customerWallet/definePercentagePurchase")]
        public async Task<ActionResult> definePercentagePurchase(DefinePercentagePurchaseMasterDto dto)
        {
            var res = await _custonerWalletService.definePercentagePurchase(dto);
            return Ok(res);
        }

        [HttpGet("/customerWallet/findAllAgents")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgents(Ulid? idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("AGENT", idAgency, page, limit);

        }

        [HttpGet("/customerWallet/findAllAgentsByAgency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgentsByAgency(Ulid idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllAgentsByAgency(idAgency, page, limit);

        }

    }
}
