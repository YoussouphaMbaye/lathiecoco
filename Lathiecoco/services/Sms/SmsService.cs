using RestSharp;
using Lathiecoco.models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Lathiecoco.models.notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Lathiecoco.services.Sms
{
    public class SmsService
    {
        private readonly IConfiguration _configuration;
        public SmsService(IConfiguration configuration) {

            _configuration = configuration;
           }

      public async  Task<ResponseBody<tokenResponse>> generateToken()
        {
            ResponseBody<tokenResponse> rp = new ResponseBody<tokenResponse>();
            var baseUrl = _configuration["SmsNotification:Url"];
            var senderPhone = _configuration["SmsNotification:OrangeSender"];

            var client = new RestClient(baseUrl);
            var request = new RestRequest("/oauth/v3/token", Method.Post);
            request.AddHeader("Authorization", "Basic " + _configuration["SmsNotification:BearerTokenOrange"]);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
           
            request.AddBody("grant_type=client_credentials");

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                tokenResponse ?  tokenResponse =  JsonConvert.DeserializeObject<tokenResponse>(content);
              
                rp.IsError = false;
                rp.Msg = "success";
                rp.Code = 200;
                rp.Body = tokenResponse;



            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Code = 500;
            }

          

            return rp;
        }
        public async Task<ResponseBody<string>> sendSms(string phoneNumber , string message)
        {
            ResponseBody<tokenResponse> generateToken = await this.generateToken();

            ResponseBody<string> rp = new ResponseBody<string>();

            var baseUrl = _configuration["SmsNotification:Url"];
            var senderPhone = _configuration["SmsNotification:OrangeSender"];
            string sendername = _configuration["SmsNotification:SenderName"];
            var client = new RestClient(baseUrl);
       
            var request = new RestRequest("/smsmessaging/v1/outbound/tel%3A%2B" + senderPhone + "/requests", Method.Post);
            request.AddHeader("Authorization", "Bearer " + generateToken.Body.access_token );
            request.AddHeader("Content-Type", "application/json");
          
           
            request.RequestFormat = DataFormat.Json;

          
            string str = "{";
            str += "\"outboundSMSMessageRequest\":{";
            str += "\"address\": \"tel:+224" + phoneNumber + "\",";
            str += "\"senderAddress\": \"tel:+" + senderPhone + "\",";
            str += "\"senderName\": \"" + sendername + "\",";
            str += "\"outboundSMSTextMessage\":{";
            str += "\"message\": \"" + message + "\"";
            str += "}}}";

            request.AddJsonBody(str);
            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    rp.Code = 200;
                    rp.IsError = false;
                    rp.Msg = "success";
                    rp.Body = content.ToString();

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "failed";
                    rp.Body = content;
                }
            }
            catch (Exception er)
            {
                rp.IsError = true;
                rp.Msg = "failed";
                rp.Body = er.Message;

            }


            return rp;

        }
    }
}
