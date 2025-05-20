using Lathiecoco.models.notifications;
using Lathiecoco.models;
using Lathiecoco.repository.Orange;

namespace Lathiecoco.services.Mtn
{
    public class MtnPaymentNotificationService
    {
         private readonly IConfiguration _configuration;
            public MtnPaymentNotificationService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

          

            public async Task<ResponseBody<string>> mtnNotificationsHandler(Notifications? om)
            {
                ResponseBody<string> rp = new ResponseBody<string>();
                rp.IsError = false;
                rp.Code = 200;
                rp.Msg = " Bonjour tout le monde !!!!!";



                return rp;
            }



        }
    }
