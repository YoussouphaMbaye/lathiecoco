using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.models.conlog;
using Lathiecoco.models.mtn;
using Lathiecoco.models.notifications;
using Lathiecoco.models.orange;
using Lathiecoco.repository;
using Lathiecoco.repository.Conlog;
using Lathiecoco.repository.Mtn;
using Lathiecoco.repository.Orange;
using Lathiecoco.services.Conlog;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Reflection;

namespace Lathiecoco.services
{
    //P paid transaction
    //F Faild transaction
    //W Pending transaction
    public class BillerInvoiceService : BilllerInvoiceRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly CustomerWalletRep _customerWalleServ;
        private readonly FeesSendRep _feesSendServ;
        private readonly PaymentModeRep _paymentModeServ;
        private readonly EDGrep _EdgRep;
        private readonly MtnRep _mtnRep;
        private readonly OrangeRep _orangeRep;
        public BillerInvoiceService(CatalogDbContext CatalogDbContext,
        CustomerWalletRep customerWalleServ,

        FeesSendRep feesSendServ,
        PaymentModeRep paymentModeServ,
        MtnRep mtnRep,
        OrangeRep orangeRep,
        EDGrep edgServices)
        {
            _CatalogDbContext = CatalogDbContext;
            _customerWalleServ = customerWalleServ;
            _feesSendServ = feesSendServ;
            _paymentModeServ = paymentModeServ;
            _EdgRep = edgServices;
            _mtnRep = mtnRep;
            _orangeRep = orangeRep;
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
                rp.Code = 400;
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

        public async Task<ResponseBody<BillerInvoice>> updateBillerInvoiceToPaid(String code)
        {
            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {

                BillerInvoice bl = await _CatalogDbContext.BillerInvoices.Include(c => c.PaymentModeObj).Include(c => c.CustomerWallet).Where(c => c.InvoiceCode == code).FirstOrDefaultAsync();
                if (bl != null)
                {
                    bl.InvoiceStatus = "P";

                    //paid from cg

                    try
                    {
                        //cg paid
                        //shoold change
                        
                        EdgPayment pay= new EdgPayment();
                        pay.montant = bl.AmountToPaid;

                        pay.numCompteur = bl.BillerReference;
                        
                        ResponseBody<AccountPaymentServicesEdg> rpAsp = await _EdgRep.payCustomer(pay);
                       
                        if (rpAsp.IsError) { 
                            rp.IsError = true;
                            rp.Msg= rpAsp.Msg;
                            rp.Code = 003;
                            return rp;
                        }
                        bl.ReloadBiller = rpAsp.Body.token.Split("|")[0];
                        bl.NumberOfKw= Convert.ToDouble(rpAsp.Body.EnergyCoast);
                        
                       
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
                    rp.Msg = "Biller with code " + code + " not found";
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
                        
                        ResponseBody<AccountPaymentServicesEdg> rpAsp = await _EdgRep.payCustomer(pay);
                       
                        if (rpAsp.IsError) { 
                            rp.IsError = true;
                            rp.Msg= rpAsp.Msg;
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

        public async Task<ResponseBody<BillerInvoice>> insertBillerInvoice(BodyBillerDto biller)
        {

            ResponseBody<BillerInvoice> rp = new ResponseBody<BillerInvoice>();
            try
            {
                //catch
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
                        if (!customer.IsActive)
                        {
                            rp.IsError = true;
                            rp.Body = null;
                            rp.Msg = "You account is not active";
                            rp.Code = 320;
                            return rp;
                        }
                        if (customer.IsBlocked)
                        {
                            rp.IsError= true;
                            rp.Body = null;
                            rp.Code = 322;
                            rp.Msg = "Your account is blocked";
                            return rp;
                        }

                        }
                        else
                        {
                            rp.Body = null;
                            rp.IsError= true;
                            rp.Msg = cusRep.Msg;
                            rp.Code = cusRep.Code;
                            return rp;
                        }
                    }
                    else
                    {
                        rp.Body = null;
                        rp.IsError= true;
                        rp.Code = cusRep.Code;
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
                            rp.Body = null;
                            rp.IsError = true;
                            rp.Msg = paymentModeRp.Msg;
                            rp.Code= paymentModeRp.Code;
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
                            rp.Code= feeSendBody.Code;  
                            return rp;
                            
                        }
                    }
                   
                    BillerInvoice invoice = new BillerInvoice();
                    invoice.IdReference=Guid.NewGuid();

                    AccountingOpWallet acw = new AccountingOpWallet();

                    double ConvertToUnixTimestamp(DateTime date)
                        {
                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        TimeSpan diff = date.ToUniversalTime() - origin;
                        return Math.Floor(diff.TotalSeconds);
                    }
                    string dayToday = ConvertToUnixTimestamp(DateTime.UtcNow).ToString();
                    invoice.IdBillerInvoice= Ulid.NewUlid();
                    invoice.InvoiceCode = "F" + GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                    invoice.PaymentMode = paymentMode1.Name.ToString();
                    invoice.FkIdPaymentMode = paymentMode1.IdPaymentMode;
                    invoice.FkIdCustomerWallet = customer.IdCustomerWallet;
                    invoice.BillerReference=biller.BillerReference;
                    invoice.InvoiceStatus = "P"; 
                    invoice.AmountToPaid = biller.AmountToPaid;
                    invoice.CustomerWallet = null;
                    invoice.FkIdFeeSend = feeSend.IdFeeSend;
                    invoice.CreatedDate = DateTime.UtcNow;
                    invoice.UpdatedDate = DateTime.UtcNow;
                    invoice.ReloadBiller = "";

                    //calculer fee amountToSend amountToPaid
                    //float aa = 0.01f;
                    //Console.WriteLine(aa + " " + 0.01 * 100);
                    //Console.WriteLine(0.01f * 100);
                    double amountTopaid = 0;
                    double amountToSend = biller.AmountToPaid;
                    if (feeSend.MinAmount < amountToSend && feeSend.MaxAmount > amountToSend)
                    {
                        
                            if (feeSend.FixeCsFee > 0)
                            {
                                amountTopaid = ((double)feeSend.FixeCsFee) + amountToSend;
                                invoice.FeesAmount = (double)feeSend.FixeCsFee;
                            }
                            else
                            {
                                decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                                amountToSend = (double)biller.AmountToPaid;

                                amountTopaid = ((double)feeForSendPercent * amountToSend) + amountToSend;
                                invoice.FeesAmount = (double)feeForSendPercent * amountToSend;
                            }
                    
                    
                    }else
                    {
                      rp.IsError = true;
                      rp.Code = 305;
                      return rp;
                    }
                    
                    
                    //Console.WriteLine(feeForSendPercent + "*" + amountToSend + "=" + amountTopaid);
                    //ac.AmountToSend = (amountToSend * (double)corridor.Rate ) + ((amountToSend * (double)corridor.Rate) * (double)feeSend.PercentAgFee);
                    invoice.AmountToPaid = amountTopaid;
              
                    var transaction = _CatalogDbContext.Database.BeginTransaction();
                    try
                    {

                        //update accounting sender

                        if (customer.Accounting != null)
                        {
                            Accounting senderAccounting = customer.Accounting;

                            if (paymentMode1.Name == "WF")
                                {
                                if (senderAccounting.Balance < amountTopaid)
                                {
                                    rp.Msg = "Your Balance is low";
                                    rp.IsError = true;
                                    rp.Body = null;
                                    rp.Code = 340;
                                    return rp;
                                }

                                //paid from cg

                                try
                                {
                               
                                    //cg paid
                                    //shoold change

                                    EdgPayment pay = new EdgPayment();
                                    pay.montant = invoice.AmountToPaid;

                                    pay.numCompteur = invoice.BillerReference;

                                    ResponseBody<AccountPaymentServicesEdg> rpAsp = await _EdgRep.payCustomer(pay);

                                    if(rpAsp.IsError) {
                                        rp.IsError = true;
                                        rp.Msg = "error of remote server (CG)!";
                                        rp.Code = 003;
                                        return rp;
                                    }
                                    invoice.ReloadBiller = rpAsp.Body.token.Split("|")[0];
                                    invoice.NumberOfKw = Convert.ToDouble(rpAsp.Body.EnergyCoast);

                                
                               
                            }
                                catch (Exception ex)
                                {
                                    rp.IsError = true;
                                    rp.Msg = "error of remote server (CG)!";
                                    rp.Code = 003;
                                    return rp;

                                }

                                senderAccounting.Balance = senderAccounting.Balance - amountTopaid;
                                _CatalogDbContext.Update(senderAccounting);
                                await _CatalogDbContext.SaveChangesAsync();
                                acw.DeBited = amountTopaid;
                            }
                            else
                            {
                                acw.DeBited = 0;
                            }
                            
                            acw.IdAccountingOperation= Ulid.NewUlid();
                            acw.FkIdAccounting = senderAccounting.IdAccounting;
                            acw.Credited = 0;
                            
                            acw.CreatedDate = DateTime.UtcNow;
                            acw.UpdatedDate = DateTime.UtcNow;
                            acw.PaymentMode = paymentMode1.Name;
                            acw.FkIdBillerInvoice = invoice.IdBillerInvoice;
                            acw.NewBalance = senderAccounting.Balance;
                           
                        }
                        else
                        {
                            rp.IsError = true; 
                            rp.Msg = "No Sender BALANCE";
                            rp.Code = 400;
                            transaction.Rollback();
                            return rp;
                        }

                        var isRemotePay = false;

                        if (paymentMode1.Name == "OM")
                        {
                            isRemotePay = true;
                            try
                            {
                            invoice.InvoiceStatus = "W";
                            OrangePaymentMethod rq = new OrangePaymentMethod();
                            rq.transactionId = invoice.IdReference.ToString();
                            rq.amount = invoice.AmountToPaid;
                            rq.phoneNumber = cusRep.Body.Phone;

                            ResponseBody<Notifications> r = await _orangeRep.transactionsProcess(rq);
                            if (r.IsError)
                            {
                                rp.IsError = true;
                                rp.Msg = "error of remote server (OM)!";
                                rp.Code = 001;
                                return rp;
                            }
                        }
                        catch (Exception ex)
                            {
                                transaction.Rollback();
                         
                                rp.IsError = true;
                                rp.Msg = "error of remote server (OM)!";
                                rp.Code = 001;
                                return rp;
                            
                            }
                        } else if (paymentMode1.Name == "MTN")
                        {
                            isRemotePay = true;
                            try
                            {
                                invoice.InvoiceStatus = "W";
                                mtnPaymentRequest rq=new mtnPaymentRequest();
                                rq.partnerId = invoice.IdReference.ToString();
                                rq.amount = invoice.AmountToPaid.ToString();
                                rq.phoneNumber = "224"+cusRep.Body.Phone;
                           
                                ResponseBody<string> r = await _mtnRep.MtnTransactionProcess(rq);
                                if(r.IsError)
                                {
                                   
                                    rp.IsError = true;
                                    rp.Msg = "error of remote server (MTN)!";
                                    rp.Code = 002;
                                    return rp;
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();

                                rp.IsError = true;
                                rp.Msg = "error of remote server (MTN)!";
                                rp.Code = 002;
                                return rp;
                                
                            }
                        }
                        
                    //inserer tansactions
                    _CatalogDbContext.BillerInvoices.Add(invoice);
                    await _CatalogDbContext.SaveChangesAsync();

                    await _CatalogDbContext.AccountingOpWallets.AddAsync(acw);
                    await _CatalogDbContext.SaveChangesAsync();

                    transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        rp.IsError = true;
                        rp.Msg = ex.Message;
                        rp.Code = 400;
                        return rp;
                    }

                    rp.Body = invoice;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Code = 400;
                rp.Body = null;
            }
            return rp;
            
        }
        public async Task<ResponseBody<List<BillerAmountByAgentDto>>> billerByAgentSumBiller(DateTime begenDate,DateTime endDate,Ulid? idAgent,Ulid? fkIdAgency)
        {
            ResponseBody<List<BillerAmountByAgentDto>> rp = new ResponseBody<List<BillerAmountByAgentDto>>();
            try
            {
                var grouped = (idAgent != null && fkIdAgency != null) ? _CatalogDbContext.BillerInvoices
               .Include(i => i.CustomerWallet)
               .Include(i => i.CustomerWallet.Agency)
               .Where(i => i.CustomerWallet.Profile == "AGENT")
               .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
               .Where(i => i.FkIdCustomerWallet == idAgent && i.CustomerWallet.FkIdAgency == fkIdAgency)
               : (idAgent != null && fkIdAgency == null) ?
               _CatalogDbContext.BillerInvoices
               .Include(i => i.CustomerWallet)
               .Include(i => i.CustomerWallet.Agency)
               .Where(i => i.CustomerWallet.Profile == "AGENT")
               .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
               .Where(i => i.FkIdCustomerWallet == idAgent)
               : (idAgent == null && fkIdAgency != null) ? _CatalogDbContext.BillerInvoices
                   .Include(i => i.CustomerWallet)
                   .Include(i => i.CustomerWallet.Agency)
                   .Where(i => i.CustomerWallet.Profile == "AGENT")
                   .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
                   .Where(i => i.CustomerWallet.FkIdAgency == fkIdAgency)
                   : _CatalogDbContext.BillerInvoices
                   .Include(i => i.CustomerWallet)
                   .Include(i => i.CustomerWallet.Agency)
                   .Where(i => i.CustomerWallet.Profile == "AGENT")
                   .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate);
                   
                   

                var groupedData=await  grouped.GroupBy(e => new { e.FkIdCustomerWallet })
                   .Select(g => new
                   {
                       count = g.Count(),
                       code = g.Max(c => c.CustomerWallet.Code),
                       Phone = g.Max(c => c.CustomerWallet.Phone),
                       TotalBillerAmount = g.Sum(e => e.AmountToPaid),
                       LastName = g.Max(c => c.CustomerWallet.LastName),
                       Agency = g.Max(c => c.CustomerWallet.Agency.code),
                       FirstName = g.Max(c => c.CustomerWallet.FirstName),
                       MiddleName = g.Max(c => c.CustomerWallet.MiddleName),
                   }).ToArrayAsync();

                if (groupedData != null)
                {
                    //rp.Body = groupedData;
                    List<BillerAmountByAgentDto> list= new List<BillerAmountByAgentDto>();
                    foreach (var i in groupedData)
                    {
                        BillerAmountByAgentDto b= new BillerAmountByAgentDto();
                        b.Count = i.count;
                        b.code = i.code;
                        b.Phone = i.Phone;
                        b.LastName = i.LastName;
                        b.FirstName = i.FirstName;
                        b.MiddleName = i.MiddleName;
                        b.TotalBillerAmount= i.TotalBillerAmount;
                        b.Agency = i.Agency;
                        list.Add(b);
                    }
                    rp.Body = list;
                }
                else
                {
                    rp.Body = new List<BillerAmountByAgentDto>();
                }
                

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg=ex.Message;
                rp.Code = 400;
            }
            
            return rp;
            

        }
        public async Task<ResponseBody<List<BillerInvoice>>> searcheBillerInvoice(string? idPaymentMode, string? code, DateTime? beginDate, DateTime? endDate,String? phone,String? billerReference,string? invoiceStatus, int page, int limit)
        {
            ResponseBody<List<BillerInvoice>> rp = new ResponseBody<List<BillerInvoice>>();
            try {
                DateTime myDateTime = DateTime.UtcNow;
                string dateNow = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if(endDate!=null) {
                    dateNow = ((DateTime)endDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                string query = $"Select * FROM \"BillerInvoices\" ";
            
                query += $"where  \"CreatedDate\" <'{dateNow}' ";
            
                if (idPaymentMode != null)
                {
                    query += $"and \"FkIdPaymentMode\" = '{idPaymentMode}' ";
                }
                if (invoiceStatus != null)
                {
                    query += $"and \"InvoiceStatus\" = '{invoiceStatus}' ";
                }
                if (code != null) 
                {
                    query += $"and  \"InvoiceCode\" = '{code}' ";
                }

                if (billerReference != null)
                {
                    query += $"and  \"BillerReference\" = '{billerReference}' ";
                }

                if (code != null)
                {
                    query += $"and  \"InvoiceCode\" = '{code}' ";
                }
                if (beginDate != null)
                {
                    string beginDateTostring = ((DateTime)beginDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    query += $"and \"CreatedDate\" > '{beginDateTostring}' ";
                }
                //query += $";";

                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.BillerInvoices != null)
                {
                    var totalItems = 
                    phone != null ? _CatalogDbContext.BillerInvoices.FromSqlRaw(query)
                        .Where(b => b.CustomerWallet.Phone == phone).Count()
                        : _CatalogDbContext.BillerInvoices.FromSqlRaw(query).Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalItems / limit);
                    if (phone != null)
                    {
                        phone = phone.Trim().Replace(" ", "");
                        ;
                    }
                    var ps = phone!=null? await _CatalogDbContext.BillerInvoices.FromSqlRaw(query).Include(b => b.CustomerWallet)
                        .Where(b => b.CustomerWallet.Phone == phone).OrderByDescending(x=>x.UpdatedDate).ToListAsync()
                        :await _CatalogDbContext.BillerInvoices.FromSqlRaw(query).Include(b => b.CustomerWallet).OrderByDescending(x => x.UpdatedDate)
                        .Skip(skip).Take(limit).ToListAsync();
                    
                    
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
            catch (Exception ex) {
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.Message;
            }

            return rp;

        }

        
    }
}
