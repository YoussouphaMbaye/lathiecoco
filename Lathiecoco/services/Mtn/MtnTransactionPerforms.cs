using Lathiecoco.models;
using Lathiecoco.models.conlog;
using Lathiecoco.models.mtn;
using Lathiecoco.models.orange;
using Lathiecoco.repository;
using Lathiecoco.repository.Conlog;
using Lathiecoco.repository.Mtn;
using Lathiecoco.services.Sms;
using Microsoft.EntityFrameworkCore;
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
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly EDGrep _edgrep;

        public MtnTransactionPerforms(IConfiguration configuration, 
            CatalogDbContext CatalogDbContext,
            EDGrep edgrep)
        {
            _configuration = configuration;
            _CatalogDbContext = CatalogDbContext;
            _edgrep = edgrep;
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

        public async Task<ResponseBody<string>> mtnMoneyNotificationsHandler(requestToPay rp)
        {
            ResponseBody<string> rpr = new ResponseBody<string>();
            rpr.IsError = false;
            rpr.Code = 200;
            rpr.Msg = " Bonjour tout le monde !!!!!";
            Console.WriteLine(rp.externalId);
            Console.WriteLine(rp.status);
            Console.WriteLine(rp.ToString());
            if (rp.status == "SUCCESS")
            {
                await updateBillerInvoiceToPaidByIdRef(new Guid(rp.externalId));
            }
            
            return rpr;
        }
        public async Task<ResponseBody<BillerInvoice>> updateBillerInvoiceToPaidByIdRef(Guid idRef)
        {
            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {

                BillerInvoice bl = await _CatalogDbContext.BillerInvoices.Include(c => c.PaymentModeObj).Include(c => c.CustomerWallet).Where(c => c.IdReference == idRef).FirstOrDefaultAsync();
                if (bl != null)
                {
                    bl.InvoiceStatus = "P";

                    //paid from cg

                    try
                    {
                        //cg paid
                        //shoold change

                        EdgPayment pay = new EdgPayment();
                        pay.montant = bl.AmountToPaid;
                        pay.numCompteur = bl.BillerReference;

                        ResponseBody<AccountPaymentServicesEdg> rpAsp = await _edgrep.payCustomer(pay);

                        if (rpAsp.IsError)
                        {
                            rp.IsError = true;
                            rp.Msg = rpAsp.Msg;
                            rp.Code = 003;
                            return rp;
                        }
                        bl.ReloadBiller = rpAsp.Body.token.Split("|")[0];
                        bl.NumberOfKw = Convert.ToDouble(rpAsp.Body.EnergyCoast);

                    }
                    catch (Exception ex)
                    {
                        rp.Code = 003;
                        rp.IsError = true;
                        bl.InvoiceStatus = "F";
                        rp.Msg = "error of remote server (CG)!";

                    }

                    _CatalogDbContext.BillerInvoices.Update(bl);
                    await _CatalogDbContext.SaveChangesAsync();

                    rp.Body = bl;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Biller with idReference " + idRef + " not found";
                    rp.Code = 460;
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

    }
}
