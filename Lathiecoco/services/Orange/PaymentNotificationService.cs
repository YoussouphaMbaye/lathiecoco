using Lathiecoco.models;
using Lathiecoco.models.conlog;
using Lathiecoco.models.notifications;
using Lathiecoco.repository;
using Lathiecoco.repository.Conlog;
using Lathiecoco.repository.Orange;
using Lathiecoco.services.Sms;
using Microsoft.EntityFrameworkCore;

namespace Lathiecoco.services.Orange
{
    public class PaymentNotificationService : paymentNotificationsRep
    {
        private readonly IConfiguration _configuration;
    
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly EDGrep _edgrep;
        public PaymentNotificationService(IConfiguration configuration,
            CatalogDbContext CatalogDbContext,
            EDGrep eDGrep) {
          _configuration = configuration;
            _edgrep = eDGrep;
            _CatalogDbContext = CatalogDbContext;
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
            
            //a changer
            if (om.status == "SUCCESS")
            {
                Console.WriteLine(om.status);
                await updateBillerInvoiceToPaidByIdRef(new Guid(om.transactionData.transactionId));
            }else if(om.status == "FAILED")
            {
                await updateBillerInvoiceToFailedByIdRef(new Guid(om.transactionData.transactionId));
            }

            return  rp;
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
                        bl.BillerUserName= rpAsp.Body.CustomerName;

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

        public async Task<ResponseBody<BillerInvoice>> updateBillerInvoiceToFailedByIdRef(Guid idRef)
        {
            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {

                BillerInvoice bl = await _CatalogDbContext.BillerInvoices.Include(c => c.PaymentModeObj).Include(c => c.CustomerWallet).Where(c => c.IdReference == idRef).FirstOrDefaultAsync();
                if (bl != null)
                {
                    bl.InvoiceStatus = "F";

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
