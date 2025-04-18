﻿using Lathiecoco.models;
using Lathiecoco.models.notifications;
using Lathiecoco.repository.Orange;
using Lathiecoco.services.Sms;

namespace Lathiecoco.services.Orange
{
    public class PaymentNotificationService : paymentNotificationsRep
    {
        private readonly IConfiguration _configuration;
        public PaymentNotificationService(IConfiguration configuration) {
          _configuration = configuration;
        }

        public Task<ResponseBody<string>> mtnMoneyNotificationsHandler(string pm)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBody<string>> orangeMoneyNotificationsHandler(Notifications? om)
        {
            ResponseBody<string> rp = new ResponseBody<string>();
            rp.IsError = false;
            rp.Code = 200;
            rp.Msg = " Bonjour tout le monde !!!!!";
            rp.Body = om.message;


            return  rp;
        }

      

        
    }

}
