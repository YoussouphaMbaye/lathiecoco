using Lathiecoco.models;
using Lathiecoco.models.conlog;
using Microsoft.AspNetCore.Mvc;
using Lathiecoco.services.Conlog;
using Lathiecoco.repository.Conlog;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdgController : ControllerBase
    {
        private readonly EDGrep _EdgRep;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environnement;


        public EdgController(EDGrep edgServices, IConfiguration configuration, IWebHostEnvironment environnement)
        {
            _EdgRep = edgServices;
            _configuration = configuration;
            _environnement = environnement;
        }

        [HttpPost("/customerCheck")]
        public async Task<ResponseBody<_customer>> EdgCheckCustomer(string numCompteur)
        {
            
            return await _EdgRep.confirmCustomer(numCompteur);

        }

        [HttpPost("/payment")]
        public async Task<ResponseBody<AccountPaymentServicesEdg>> EdgPayment(EdgPayment pay)
        {

            return await _EdgRep.payCustomer(pay);

        }
    }
}
