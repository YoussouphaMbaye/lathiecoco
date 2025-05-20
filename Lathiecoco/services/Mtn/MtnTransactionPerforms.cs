using Lathiecoco.models;
using Lathiecoco.models.mtn;
using Lathiecoco.models.orange;
using Lathiecoco.repository.Mtn;
using Lathiecoco.services.Sms;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;
using System.Net;

namespace Lathiecoco.services.Mtn
{
    public class MtnTransactionPerforms : MtnRep
    {
        private readonly IConfiguration _configuration;

        public MtnTransactionPerforms(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        async Task<ResponseBody<mtnTokenGenerate>> generateMtnToken()
        {
            ResponseBody<mtnTokenGenerate> rp = new ResponseBody<mtnTokenGenerate>();
           
            var baseUrl = _configuration["Mtn:Url"];
            var username = _configuration["Mtn:username"];
            var password = _configuration["Mtn:password"];
            var subscribToken = _configuration["Mtn:subscriptionkey"];
            var auth = _configuration["Mtn:Authentication"];
            var options = new RestClientOptions(baseUrl)
            {
               // Authenticator = new HttpBasicAuthenticator(username, password),
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/collection/token/", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", auth);
            request.AddHeader("Ocp-Apim-Subscription-Key", subscribToken);
            
          
            

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                if (response.IsSuccessStatusCode) {
                    var  mtn =   JsonConvert.DeserializeObject<mtnTokenGenerate>(response.Content);
                   
                    rp.Code = 200;
                    rp.Body = mtn;
                    rp.IsError = false;
                }
                else
                {
                    rp.IsError = true;
                    rp.Code = 500;
                    rp.Msg = "Error login";
                }
               
              

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
             
                
            }

           

            return rp;
        }

        public async Task<ResponseBody<string>> MtnTransactionProcess(mtnPaymentRequest mtn)
        {
            ResponseBody<string> rp = new ResponseBody<string>();

            var token = await generateMtnToken();

            if (!token.IsError)
            {

                var baseUrl = _configuration["Mtn:Url"];
                var partyIdType = _configuration["Mtn:partyIdType"];
                var subscribToken = _configuration["Mtn:subscriptionkey"];
                var xtargetEnvironment = _configuration["Mtn:X-Target-Environment"];

                var client = new RestClient(baseUrl);
                var request = new RestRequest("/collection/v1_0/requesttopay", Method.Post);
                request.AddHeader("Authorization", "Bearer " + token.Body.access_token);
               
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Reference-Id", mtn.partnerId.Trim());
                request.AddHeader("X-Target-Environment", xtargetEnvironment);
                request.AddHeader("Ocp-Apim-Subscription-Key", subscribToken);
               
                Dictionary<string, object> payer = new Dictionary<string, object> {
                 { "partyIdType",partyIdType} ,
                 { "partyId", mtn.phoneNumber},
               
                 };

                Dictionary<string, object> reqBody = new Dictionary<string, object> {
                { "amount",mtn.amount } ,
                { "currency", "GNF"},
                { "externalId", mtn.partnerId.Trim() } ,
                { "payer", payer },
                { "payerMessage","string" },
                { "payeeNote","string" },
                  };

                string reqBodyJson = JsonConvert.SerializeObject(reqBody);
                request.AddStringBody(reqBodyJson, DataFormat.Json);
               
                try
                {
                    RestResponse response = await client.ExecuteAsync(request);
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                       
                        rp.IsError = false;
                        rp.Code = 201;
                        rp.Msg = "Sent";
                      

                    }

                    else
                    {
                        rp.IsError = true;
                        rp.Code = 400;
                        rp.Msg = "Not Sent";
                        rp.Body = content;
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




            return rp;

        }
    }
}
