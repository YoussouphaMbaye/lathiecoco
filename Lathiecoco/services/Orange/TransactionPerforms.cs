using Lathiecoco.models;
using Lathiecoco.models.notifications;
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

        public async Task<ResponseBody<tokenResponse>> generateToken()
        {
            ResponseBody<tokenResponse> rp = new ResponseBody<tokenResponse>();
            var baseUrl = _configuration["SmsNotification:url"];
            var client = new RestClient(baseUrl);
            var request = new RestRequest("oauth/v3/token", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", _configuration["SmsNotification:BearerTokenCallBack"]);

            request.AddBody("grant_type=client_credentials");

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                tokenResponse? tokenResponse = JsonConvert.DeserializeObject<tokenResponse>(content);

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

        public async Task<ResponseBody<Notifications>> transactionsProcess(OrangePaymentMethod trans)
        {
            ResponseBody<Notifications> rp = new ResponseBody<Notifications>();
            Notifications transactionsOrange =  new Notifications();
            SmsService Orange = new SmsService(_configuration);
            try
            {
                ResponseBody<tokenResponse>? token = await generateToken();

                if (token != null)
                {
                    if (!token.IsError)
                    {

                        var baseUrl = _configuration["SmsNotification:ProdUrl"];
                        // var baseUrl = _configuration["SmsNotification:Url"];
                        var posId = _configuration["SmsNotification:PosId"];
                        var peerIdType = _configuration["SmsNotification:peerIdType"];
                        var currency = _configuration["SmsNotification:currency"];

                        var client = new RestClient(baseUrl);
                        var request = new RestRequest("transactions/cashout", Method.Post);
                        request.AddHeader("Authorization", "Bearer " + token.Body.access_token);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Accept", "application/json");

                        Dictionary<string, object> reqBody = new Dictionary<string, object> {
                { "peerId",trans.phoneNumber.Trim() } ,
                { "peerIdType", peerIdType},
                { "amount",trans.amount } ,
                { "currency", currency },
                { "posId", posId },
                { "transactionId",trans.transactionId },
                   };



                        string reqBodyJson = JsonConvert.SerializeObject(reqBody);
                        request.AddStringBody(reqBodyJson, DataFormat.Json);

                        try
                        {
                            RestResponse response = await client.ExecuteAsync(request);
                            var content = response.Content;
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                transactionsOrange = JsonConvert.DeserializeObject<Notifications>(content);
                                rp.IsError = false;
                                rp.Code = 201;
                                rp.Msg = "Sent";
                                rp.Body = transactionsOrange;

                            }
                            else
                            {
                                rp.IsError = true;
                                rp.Code = 403;
                                rp.Msg = content.ToString();

                            }
                        }
                        catch (Exception ex)
                        {
                            rp.IsError = true;
                            rp.Code = 500;
                            rp.Msg = ex.Message;

                        }

                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Code = 500;
                        rp.Msg = token.Msg;
                    }

                }
            }
            catch(Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;

            }
           
           
           

            return rp;
        }
    }
}
