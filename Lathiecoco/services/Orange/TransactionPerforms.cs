using Lathiecoco.models;
using Lathiecoco.models.orange;
using Lathiecoco.repository.Orange;
using Lathiecoco.services.Sms;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.NetworkInformation;

namespace Lathiecoco.services.Orange
{
    public class TransactionPerforms : OrangeRep
    {
        private readonly IConfiguration _configuration;
        public TransactionPerforms(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ResponseBody<string>> transactionsProcess(TransactionsOrange trans)
        {
            ResponseBody<string> rp = new ResponseBody<string>();
            SmsService Orange = new SmsService(_configuration);
            var token = await Orange.generateToken();

            var baseUrl = _configuration["SmsNotification:Url"];
            var peerIdType = _configuration["SmsNotification:peerIdType"];

            var client = new RestClient(baseUrl);
            var request = new RestRequest("transactions/merchpay", Method.Post);
            request.AddHeader("Authorization", "Bearer " + token.Body.access_token );
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            Dictionary<string, object> reqBody = new Dictionary<string, object> {
                { "peerId",trans.peerId.Trim() } ,
                { "peerIdType", peerIdType.Trim()},
                { "amount",trans.amount } ,
                { "currency", trans.currency.Trim() },
                { "posId",trans.posId.Trim() },
                { "transactionId",trans.transactionId },
            };

            string reqBodyJson = JsonConvert.SerializeObject(reqBody);
            request.AddStringBody(reqBodyJson, DataFormat.Json);

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = JsonConvert.DeserializeObject<RestResponse>(content);
                    rp.IsError = false;
                    rp.Code = 201;
                    rp.Msg = "Sent";
                    rp.Body = responseJson.Content;
                
                }
            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 500;
                rp.Msg = ex.Message;

            }

            return rp;
        }
    }
}
