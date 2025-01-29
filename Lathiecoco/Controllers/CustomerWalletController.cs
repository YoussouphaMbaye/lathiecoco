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
        [HttpPost("/customer-wallet/login")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletLogin([FromBody] BodyLoginDto cu)
        {

            return await _custonerWalletService.findCustomerWalletPinContryidentityPhone(cu);

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
        [HttpPut("/customer-wallet/update-by-staff")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletUpdateByStaff([FromBody] CustomerUpdateDto cu)
        {

            return await _custonerWalletService.updateCustomerInformationsWithoutPinNumber(cu);

        }
        
        [HttpGet("/customer-wallet/find-by-id")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> findById(Ulid id)
        {

            return await _custonerWalletService.findCustomerWalletById(id);

        }
        [HttpPost("/customer-wallet/active-or-deactivate")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> activeCustomerWallet(ActiveBlockDto dto)
        {

            return await _custonerWalletService.activateWallet(dto);

        }
        [HttpPost("/customer-wallet/customer-wallet-to-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<CustomerWallet>> customerWalletToAgency(CustomerToAgencyDto dto)
        {

            return await _custonerWalletService.CustomerToAgencyDto(dto);

        }
        [HttpPost("/customer-wallet/blocke-or-debloke")]
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
        [HttpGet("/customer-wallet/customer-wallet-date-between-and-agent")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> getCustomerWalletDateBetweenAndAgent(DateTime begenDate, DateTime endDate, Ulid? idAgent, int page = 1, int limit = 10)

        {

            return await _custonerWalletService.getCustomerWalletDateBetweenAndAgent(begenDate,endDate,idAgent,page,limit);

        }

        [HttpGet("/customer-wallet/find-all-customers")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(Ulid? idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("CUSTOMER", idAgency, page, limit);

        }

        [HttpPost("/customer-wallet/define-percentage-purchase")]
        public async Task<ActionResult> definePercentagePurchase(DefinePercentagePurchaseMasterDto dto)
        {
            var res = await _custonerWalletService.definePercentagePurchase(dto);
            return Ok(res);
        }

        [HttpGet("/customer-wallet/find-all-agents")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgents(Ulid? idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllCustomerByprofile("AGENT", idAgency, page, limit);

        }

        [HttpGet("/customer-wallet/find-all-agents-by-agency")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgentsByAgency(Ulid idAgency, int page = 1, int limit = 10)
        {

            return await _custonerWalletService.findAllAgentsByAgency(idAgency, page, limit);

        }

    }
}
