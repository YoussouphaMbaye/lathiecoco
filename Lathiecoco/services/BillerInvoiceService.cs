using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lathiecoco.services
{
    public class BillerInvoiceService : BilllerInvoiceRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly CustomerWalletRep _customerWalleServ;
        private readonly FeesSendRep _feesSendServ;
        private readonly PaymentModeRep _paymentModeServ;
        public BillerInvoiceService(CatalogDbContext CatalogDbContext,
        CustomerWalletRep customerWalleServ,
        
        FeesSendRep feesSendServ,
        PaymentModeRep paymentModeServ)
        {
            _CatalogDbContext = CatalogDbContext;
            _customerWalleServ = customerWalleServ;
            _feesSendServ = feesSendServ;
            _paymentModeServ = paymentModeServ;
        }
        public async Task<ResponseBody<List<BillerInvoice>>> findAllBillerInvoice(int page = 1, int limit = 10)
        {
            ResponseBody<List<BillerInvoice>> rp = new ResponseBody<List<BillerInvoice>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.BillerInvoices != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.BillerInvoices.Count() / limit);
                    var ps = await _CatalogDbContext.BillerInvoices.Include(x => x.CustomerWallet).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<BillerInvoice>();
                    }
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;

            }
            return rp;
        }

        public async Task<ResponseBody<BillerInvoice>> findBillerInvoiceById(Ulid id)
        {
            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {


                BillerInvoice bl = await _CatalogDbContext.BillerInvoices.Include(c => c.PaymentModeObj).Include(c =>c.CustomerWallet).Where(c => c.IdBillerInvoice == id).FirstOrDefaultAsync();
                if (bl != null)
                {
                    rp.Body = bl;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Biller "+ id +" not found";
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<BillerInvoice>> insertBillerInvoice(BodyBillerDto biller)
        {

            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {
                //catch
                if (biller.PayementType == biller.PayementType)
                {
                    CustomerWallet customer = null;
                    FeeSend feeSend = null;
                    PaymentMode paymentMode1 = null;
                    //rechercher customer avec id
                    ResponseBody<CustomerWallet> cusRep = await _customerWalleServ.findCustomerWalletById(biller.IdCustomerWallet);
                    if (cusRep != null)
                    {
                        
                        if (!cusRep.IsError)
                        {
                            customer = cusRep.Body;
                            

                        }
                        else
                        {
                            rp.IsError= true;
                            rp.Msg = cusRep.Msg;
                            rp.Body = null;
                            return rp;
                        }
                    }
                    else
                    {
                        rp.IsError= true;
                        rp.Body = null;
                        return rp;
                    }
                    
                    //rechercher le mode de payement
                    ResponseBody<PaymentMode> paymentModeRp = await _paymentModeServ.findByPaymentMode(biller.PayementType);
                    if (paymentModeRp != null)
                    {
                        if (!paymentModeRp.IsError)
                        {
                            paymentMode1 = paymentModeRp.Body;  
                        }
                        else
                        {
                            rp.IsError = true;
                            rp.Msg = paymentModeRp.Msg;
                            rp.Body = null;
                            return rp;
                        }
                    }
                   
                    //rechercher fee_send idCorridor & paymentMode
                    ResponseBody<FeeSend> feeSendBody = await _feesSendServ.findWithPaymentMode(paymentMode1.IdPaymentMode);
                    if (feeSendBody != null)
                    {
                        if (!feeSendBody.IsError)
                        {
                            feeSend = feeSendBody.Body;
                        }
                        else
                        {
                            rp.IsError = true;
                            rp.Msg = feeSendBody.Msg;
                            rp.Body = null;
                            return rp;
                            
                        }
                    }
                   
                    BillerInvoice invoice = new BillerInvoice();
                    double ConvertToUnixTimestamp(DateTime date)
                    {
                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        TimeSpan diff = date.ToUniversalTime() - origin;
                        return Math.Floor(diff.TotalSeconds);
                    }
                    string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
                    invoice.IdBillerInvoice= Ulid.NewUlid();
                    invoice.InvoiceCode = "F" + GlobalFunction.ConvertToUnixTimestamp(DateTime.Now);
                    invoice.PaymentMode = paymentMode1.Name.ToString();
                    invoice.FkIdPaymentMode = paymentMode1.IdPaymentMode;
                    invoice.FkIdCustomerWallet = customer.IdCustomerWallet;
                    //shoold change
                    invoice.ReloadBiller= dayToday + dayToday.Substring(1, 3);
                    invoice.BillerReference=biller.BillerReference;
                    invoice.InvoiceStatus = "P";
                    invoice.AmountToPaid = biller.AmountToPaid;
                    invoice.CustomerWallet = null;
                    invoice.FkIdFeeSend = feeSend.IdFeeSend;
                    invoice.CreatedDate = DateTime.Now;
                    invoice.UpdatedDate = DateTime.Now;

                    //calculer fee amountToSend amountToPaid
                    float aa = 0.01f;
                    Console.WriteLine(aa + " " + 0.01 * 100);
                    Console.WriteLine(0.01f * 100);
                    double amountTopaid = 0;
                    double amountToSend = 0;
                    if (feeSend.FixeCsFee > 0)
                    {
                        amountTopaid = ((double)feeSend.FixeCsFee) + amountToSend;
                        invoice.FeesAmount = (double)feeSend.FixeCsFee;
                    }
                    else {
                        decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                        amountToSend = (double)biller.AmountToPaid;

                        amountTopaid = ((double)feeForSendPercent * amountToSend) + amountToSend;
                        invoice.FeesAmount = (double)feeForSendPercent * amountToSend;
                    }
                    
                    //Console.WriteLine(feeForSendPercent + "*" + amountToSend + "=" + amountTopaid);
                    //ac.AmountToSend = (amountToSend * (double)corridor.Rate ) + ((amountToSend * (double)corridor.Rate) * (double)feeSend.PercentAgFee);
                    invoice.AmountToPaid = amountTopaid;
              
                    var transaction = _CatalogDbContext.Database.BeginTransaction();
                    try
                    {
                        //inserer tansactions
                        _CatalogDbContext.BillerInvoices.Add(invoice);
                        await _CatalogDbContext.SaveChangesAsync();

                        //update accounting sender

                        if (customer.Accounting != null)
                        {
                            Accounting senderAccounting = customer.Accounting;
                            if (senderAccounting.Balance < amountTopaid)
                            {
                                rp.Msg = "Your Balance is low";
                                rp.IsError = true;
                                rp.Body = null;
                                return rp;
                            }
                            senderAccounting.Balance = senderAccounting.Balance - amountTopaid;
                            _CatalogDbContext.Update(senderAccounting);
                            await _CatalogDbContext.SaveChangesAsync();
                            AccountingOpWallet acw = new AccountingOpWallet();
                            acw.IdAccountingOperation= Ulid.NewUlid();
                            acw.FkIdAccounting = senderAccounting.IdAccounting;
                            acw.Credited = 0;
                            acw.DeBited = amountTopaid;
                            acw.CreatedDate = DateTime.Now;
                            acw.PaymentMode = paymentMode1.Name;
                            acw.UpdatedDate = DateTime.Now;
                            acw.FkIdBillerInvoice = invoice.IdBillerInvoice;
                            acw.NewBalance = senderAccounting.Balance;
                            await _CatalogDbContext.AccountingOpWallets.AddAsync(acw);
                            await _CatalogDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            rp.IsError = true;
                            rp.Msg = "No Sender BALANCE";
                            transaction.Rollback();
                            return rp;
                        }


                       
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        rp.IsError = true;
                        rp.Msg = "error";
                        return rp;
                    }




                    rp.Body = invoice;


                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Payment type not found";
                    rp.Body = null;
                    return rp;
                }
                



            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Body = null;
            }
            return rp;
            
        }
    }
}
