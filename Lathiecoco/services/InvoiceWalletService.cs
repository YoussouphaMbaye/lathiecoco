using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace  Lathiecoco.services
{
    public class InvoiceWalletService : InvoiceWalletRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly CustomerWalletRep _customerWalleServ;
        private readonly FeesSendRep _feesSendServ;
        
        private readonly PaymentModeRep _paymentModeServ;
        public InvoiceWalletService(CatalogDbContext CatalogDbContext,
        CustomerWalletRep customerWalleServ,
       
        FeesSendRep feesSendServ,
        PaymentModeRep paymentModeServ)
        {
            _CatalogDbContext = CatalogDbContext;
            _customerWalleServ = customerWalleServ;
            _feesSendServ=feesSendServ;
           
            _paymentModeServ= paymentModeServ;
    }
        public async Task<ResponseBody<InvoiceWallet>> addInvoiceWallet(InvoiceWallet ac)
        {
            ResponseBody<InvoiceWallet> rp = new ResponseBody<InvoiceWallet>();
            try
            {


                await _CatalogDbContext.InvoiceWallets.AddAsync(ac);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = ac;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<List<InvoiceWallet>>> findAllInvoiceWallet(int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceWallet>> rp = new ResponseBody<List<InvoiceWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceWallets != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceWallets.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceWallets.Include(x=>x.CustomerRecipient).Include(x => x.CustomerSender).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<InvoiceWallet>();
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


        public async Task<ResponseBody<InvoiceWallet>> insertInvoiceWallet(BodyPostInvoiceWallet ac)
        {
            
            CustomerWallet recipient = null;
            CustomerWallet sender = null;
            FeeSend feeSend = null;
            ResponseBody<InvoiceWallet> rp = new ResponseBody<InvoiceWallet>();
            BodyPaymentModeCorridor bcc = new BodyPaymentModeCorridor();
            PaymentMode paymentMode1=new PaymentMode();
            try
            {
                //rechercher sender can
                BodyPhoneShDto bp = new BodyPhoneShDto();
                bp.CountryIdentity = ac.PhoneIdentitySender;
                bp.Phone = ac.PhoneSender;
                ResponseBody<CustomerWallet> senderRep = await _customerWalleServ.findCustomerWalletContryidentityAndPhone(bp);
                if (senderRep != null)
                {
                    if (!senderRep.IsError)
                    {
                        sender = senderRep.Body;
                        if (sender.Accounting.Balance <= ac.AmountToSend)
                        {
                            throw new Exception("Low balance for this transaction");
                        }
                    }
                    else
                    {
                        throw new Exception(senderRep.Msg);
                    }
                }
                //rechercher destinataire
                BodyPhoneShDto bpr = new BodyPhoneShDto();
                bpr.CountryIdentity = ac.PhoneIdentityecipient;
                bpr.Phone = ac.PhoneRecipient;


                ResponseBody<CustomerWallet> resRep = await _customerWalleServ.findCustomerWalletContryidentityAndPhone(bpr);

                if (resRep != null)
                {
                    if (!resRep.IsError)
                    {
                        recipient = resRep.Body;
                    }
                    else
                    {
                        throw new Exception(resRep.Msg);
                    }
                }
                //rechercher corridor
                BodyShCorridorDto bcor = new BodyShCorridorDto();
                bcor.currencyOrigin = ac.CurrencyOrigin;
                bcor.countryDestination = ac.CountryDestination;
                bcor.countryOrigin = ac.CountryOrigin;
                bcor.currencyDestination = ac.CurrencyDestination;

                
                //rechercher cashier
                /*
                ResponseBody<Cashier> caRep = await _cashierServ.findCashierWithCode(ac.CodePayee);
                if (caRep != null)
                {
                    if (!caRep.IsError)
                    {
                        Console.WriteLine("mmmm");
                        cashier = caRep.Body;
                        bcc.idCashier = cashier.FkIdAgency;
                        Console.WriteLine("mmmm"+ cashier.FkIdAgency);
                    }
                    else
                    {
                        throw new Exception(caRep.Msg);
                    }
                }
                */
                
                //rechercher le mode de payement
                ResponseBody<PaymentMode> paymentMode = await _paymentModeServ.findByPaymentMode("WW");
                if (paymentMode != null)
                {
                    if (!paymentMode.IsError)
                    {
                        paymentMode1 = paymentMode.Body;
                        bcc.idPaymentMode=paymentMode1.IdPaymentMode;
                    }
                    else
                    {
                        throw new Exception(paymentMode.Msg);
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
                        throw new Exception(feeSendBody.Msg);
                    }
                }
                InvoiceWallet invoice = new InvoiceWallet();

                double ConvertToUnixTimestamp(DateTime date)
                {
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan diff = date.ToUniversalTime() - origin;
                    return Math.Floor(diff.TotalSeconds);
                }
                string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
                invoice.IdInvoiceWallet= Ulid.NewUlid();
                invoice.InvoiceCode = "T"+dayToday+dayToday.Substring(1,3);
                invoice.InvoiceCode2 = "";
                invoice.PaymentMode = paymentMode1.Name.ToString();
                invoice.FkIdPaymentMode=paymentMode1.IdPaymentMode;
                invoice.FkIdSender = sender.IdCustomerWallet;
                invoice.FkIdRecipient = recipient.IdCustomerWallet;
               
                invoice.InvoiceStatus = "P";
                invoice.AmountToSend = ac.AmountToSend;

                invoice.CustomerSender = null;
                invoice.CustomerRecipient = null;
                //invoice.FkIdCashierPayee = cashier.IdCashier;
                //invoice.FkIdCashierSender = cashier.IdCashier;
                //invoice.CashierSender = null;
                //invoice.CashierPayee = null;
                invoice.FkIdFeeSend=feeSend.IdFeeSend;
                invoice.CreatedDate=DateTime.Now;
                invoice.UpdatedDate=DateTime.Now;
                

                //calculer fee amountToSend amountToPaid
                float aa = 0.01f;
                Console.WriteLine(aa+" "+0.01 * 100);
                Console.WriteLine(0.01f * 100);
                decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                double amountToSend = (double)ac.AmountToSend;
                
                double amountTopaid = ((double)feeForSendPercent * amountToSend)+ amountToSend;
                Console.WriteLine(feeForSendPercent + "*"+ amountToSend + "="+ amountTopaid);
                //ac.AmountToSend = (amountToSend * (double)corridor.Rate ) + ((amountToSend * (double)corridor.Rate) * (double)feeSend.PercentAgFee);
                invoice.AmountToPaid = amountTopaid;
                double amountToReceved = amountToSend;
                invoice.FeesAmount = (double)feeForSendPercent * amountToSend;
                Console.WriteLine(amountToReceved);
                var transaction=_CatalogDbContext.Database.BeginTransaction();
                try
                {
                    //inserer tansactions
                    _CatalogDbContext.InvoiceWallets.Add(invoice);
                    await _CatalogDbContext.SaveChangesAsync();

                    //update accounting sender
                    
                    if (sender.Accounting != null) {
                        Accounting senderAccounting = sender.Accounting;
                        if (senderAccounting.Balance< amountTopaid)
                        {
                            throw new Exception("Your Balance is low");
                        }
                        senderAccounting.Balance = senderAccounting.Balance - amountTopaid;
                        _CatalogDbContext.Update(senderAccounting);
                        await _CatalogDbContext.SaveChangesAsync();
                        AccountingOpWallet acw = new AccountingOpWallet();
                        acw.FkIdAccounting = senderAccounting.IdAccounting;
                        acw.IdAccountingOperation= Ulid.NewUlid();
                        acw.Credited = 0;
                        acw.DeBited = amountTopaid;
                        acw.CreatedDate = DateTime.Now;
                        acw.PaymentMode=paymentMode1.Name.ToString();
                        acw.UpdatedDate= DateTime.Now;
                        acw.FkIdInvoice = invoice.IdInvoiceWallet;
                        acw.NewBalance = senderAccounting.Balance;
                       await _CatalogDbContext.AccountingOpWallets.AddAsync(acw);
                        await _CatalogDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = "No Sender BALANCE";
                        transaction.Rollback();
                        throw new Exception("No Sender BALANCE");
                    }
                   

                    //update accounting destination
                   
                    if (recipient.Accounting != null)
                    {
                        Accounting recipientAccounting = recipient.Accounting;
                        recipientAccounting.Balance = recipientAccounting.Balance + (amountToSend);
                        AccountingOpWallet acw = new AccountingOpWallet();
                        acw.IdAccountingOperation= Ulid.NewUlid();
                        acw.FkIdAccounting = recipientAccounting.IdAccounting;
                        acw.Credited = amountToSend;
                        acw.DeBited = 0;
                        acw.FkIdInvoice = invoice.IdInvoiceWallet;
                        acw.NewBalance = recipientAccounting.Balance;
                        acw.PaymentMode = paymentMode1.Name.ToString();
                        acw.CreatedDate = DateTime.Now;
                        acw.UpdatedDate = DateTime.Now;
                        _CatalogDbContext.Update(recipientAccounting);
                        await _CatalogDbContext.SaveChangesAsync();
                        await _CatalogDbContext.AccountingOpWallets.AddAsync(acw);
                        await _CatalogDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = "No Recepient BALANCE";
                        transaction.Rollback();
                        throw new Exception("No Recepient BALANCE");

                    }

                    transaction.Commit();

                }
                catch(Exception ex)
                {
                    rp.IsError = true;
                    rp.Msg = ex.ToString();
                }
                
               
                

                rp.Body = invoice;
            }catch(Exception ex)
            {
                
                rp.IsError = true;
                rp.Msg = ex.ToString();
            }

            //inserer operation accounting sender
            //inserer operation accounting destinataire
            return rp;
        }

        public async Task<ResponseBody<InvoiceWallet>> invoiceWalletWithId(Ulid id)
        {
            ResponseBody<InvoiceWallet>rp = new ResponseBody<InvoiceWallet>();
            
            try
            {
                InvoiceWallet inv = await _CatalogDbContext.InvoiceWallets
                    .Include(i=>i.CustomerSender)
                    .Include(i=>i.CustomerRecipient)
                    .Include(i => i.FeeSend)
                    .Include(i=>i.PaymentModeObj)
                    .Where(i=>i.IdInvoiceWallet==id)
                    .FirstOrDefaultAsync();
                rp.Body = inv;
            }
            catch(Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
            }
            return rp;
        }
    }
}
