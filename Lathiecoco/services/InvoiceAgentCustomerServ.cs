using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class InvoiceAgentCustomerServ : InvoiceAgentRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly CustomerWalletRep _customerWalleServ;
        
        private readonly FeesSendRep _feesSendServ;
        private readonly PaymentModeRep _paymentModeServ;
        public InvoiceAgentCustomerServ(CatalogDbContext CatalogDbContext,
        CustomerWalletRep customerWalleServ,
        FeesSendRep feesSendServ,
        PaymentModeRep paymentModeServ)
        {
            _CatalogDbContext = CatalogDbContext;
            _customerWalleServ = customerWalleServ;
            _feesSendServ = feesSendServ;
            _paymentModeServ = paymentModeServ;
        }
        public async Task<ResponseBody<InvoiceWalletAgent>> addInvoiceWallet(InvoiceWalletAgent ac)
        {
            ResponseBody<InvoiceWalletAgent> rp = new ResponseBody<InvoiceWalletAgent>();
            try
            {


                await _CatalogDbContext.InvoiceWalletAgents.AddAsync(ac);
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

        public async Task<ResponseBody<List<InvoiceWalletAgent>>> findAllInvoiceWallet(int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceWalletAgent>> rp = new ResponseBody<List<InvoiceWalletAgent>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceWalletAgents != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceWalletAgents.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceWalletAgents.Include(i => i.Agent).Include(i=>i.CustomerWallet).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<InvoiceWalletAgent>();
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

        public async Task<ResponseBody<InvoiceWalletAgent>> deposit(BodyInvoiceWalletCashier ac)
        {
            CustomerWallet customer = null;
            
            CustomerWallet agent = null;
            
            FeeSend feeSend = null;
            ResponseBody<InvoiceWalletAgent> rp = new ResponseBody<InvoiceWalletAgent>();
            BodyPaymentModeCorridor bcc = new BodyPaymentModeCorridor();
            PaymentMode paymentMode1 = new PaymentMode();
            try
            {
                //rechercher sender can
                BodyPhoneShDto bp = new BodyPhoneShDto();
                bp.CountryIdentity = ac.PhoneIdentityCustomerWallet;
                bp.Phone = ac.PhoneCustomerWallet;

                //rechercher Agent 

                ResponseBody<CustomerWallet> caReps = await _customerWalleServ.findCustomerWalletById(ac.IdAgent);
                if (caReps != null)
                {
                    if (!caReps.IsError)
                    {
                        agent = caReps.Body;
                        if (!agent.IsActive)
                        {
                            rp.IsError = true;
                            rp.Body = null;
                            rp.Msg = "You account is not active";
                            rp.Code = 320;
                            return rp;
                        }
                        if (agent.IsBlocked)
                        {
                            rp.IsError = true;
                            rp.Body = null;
                            rp.Msg = "Your account is blocked";
                            rp.Code = 320;
                            return rp;
                        }

                        //bcc.idCashier = cashier.FkIdAgency;
                        if (agent.Profile != "AGENT")
                        {
                            rp.IsError = true;
                            rp.Msg = "This operation is not allowed";
                            rp.Code = 370;
                            return rp;
                            //throw new Exception("This operation is not allowed");
                        }

                        if (agent.Accounting.Balance <= ac.AmountToSend)
                        {

                            rp.IsError = true;
                            rp.Msg = "Agent Low balance for this transaction";
                            rp.Code = 340;
                            return rp;
                        }
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = caReps.Msg;
                        rp.Code = 400;
                        return rp;

                    }
                }

                ResponseBody<CustomerWallet> customerRp = await _customerWalleServ.findCustomerWalletContryidentityAndPhone(bp);
                if (customerRp != null)
                {
                    if (!customerRp.IsError)
                    {
                        customer = customerRp.Body;

                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = customerRp.Msg;
                        rp.Code = customerRp.Code;
                        return rp;
                        
                    }
                }
                
                //rechercher le mode de payement
                ResponseBody<PaymentMode> paymentMode = await _paymentModeServ.findByPaymentMode("DP");
                if (paymentMode != null)
                {
                    if (!paymentMode.IsError)
                    {
                        paymentMode1 = paymentMode.Body;
                        bcc.idPaymentMode= paymentMode1.IdPaymentMode;
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = paymentMode.Msg;
                        rp.Code=paymentMode.Code;
                        return rp;
                    }
                }
                //rechercher fee_send idCorridor & idCahier
                double amountTopaid = 0;
                double amountToSend = 0;
                InvoiceWalletAgent invoice = new InvoiceWalletAgent();

                ResponseBody<FeeSend> feeSendBody = await _feesSendServ.findWithPaymentMode(paymentMode1.IdPaymentMode);
                if (feeSendBody != null)
                {
                    if (!feeSendBody.IsError)
                    {
                        feeSend = feeSendBody.Body;
                        if (feeSend.MinAmount < ac.AmountToSend && feeSend.MaxAmount > ac.AmountToSend)
                        {

                            if (feeSend.FixeCsFee > 0)
                            {
                                amountTopaid = ((double)feeSend.FixeCsFee) + ac.AmountToSend;
                                invoice.FeesAmount = (double)feeSend.FixeCsFee;
                            }
                            else
                            {
                                decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                                amountToSend = (double)ac.AmountToSend;

                                amountTopaid = ((double)Math.Ceiling(feeForSendPercent * (decimal)amountToSend)) + amountToSend;
                                invoice.FeesAmount = (double)Math.Ceiling((feeForSendPercent * (decimal)amountToSend));
                            }


                        }
                        else
                        {
                            rp.Msg = "Amount in not in defined interval!!";
                            rp.IsError = true;
                            rp.Code = 305;
                            return rp;
                        }

                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = feeSendBody.Msg;
                        rp.Code=feeSendBody.Code;
                        return rp;
                    }
                }
               

                //end

               
                invoice.IdInvoiceWalletCashier= Ulid.NewUlid();
                invoice.InvoiceCode = "D"+ GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                invoice.InvoiceCode2 = "";
                invoice.PaymentMode = paymentMode1.Name.ToString();
                invoice.FkIdPaymentMode=paymentMode1.IdPaymentMode;
                invoice.FkIdFeeSend=feeSend.IdFeeSend;
                invoice.InvoiceStatus = "P";
                invoice.FkIdCustomerWallet = customer.IdCustomerWallet;
                invoice.FkIdAgent = agent.IdCustomerWallet;
                invoice.AmountToSend = ac.AmountToSend;
                invoice.CreatedDate=DateTime.UtcNow;
                invoice.UpdatedDate = DateTime.UtcNow;

                invoice.CustomerWallet = null;
                invoice.Agent = null;
               
                //decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                //double amountToSend = (double)ac.AmountToSend;
                
                //double amountTopaid = ((double)feeForSendPercent * amountToSend) + amountToSend;
                invoice.AmountToPaid = amountTopaid;
                double amountToReceved = amountToSend;
                
                
                var transaction = _CatalogDbContext.Database.BeginTransaction();
                try
                {
                    //inserer tansactions
                    _CatalogDbContext.InvoiceWalletAgents.Add(invoice);
                    await _CatalogDbContext.SaveChangesAsync();

                    //update accounting sender

                    if (customer.Accounting != null)
                    {
                        Accounting senderAccounting = customer.Accounting;
                      
                        senderAccounting.Balance = senderAccounting.Balance + (amountToSend);

                        _CatalogDbContext.Accountings.Update(senderAccounting);
                        await _CatalogDbContext.SaveChangesAsync();

                        AccountingOpWallet acw = new AccountingOpWallet();
                        acw.IdAccountingOperation= Ulid.NewUlid();
                        acw.FkIdAccounting = senderAccounting.IdAccounting;
                        acw.Credited = amountTopaid;
                        acw.DeBited = 0;
                        acw.PaymentMode = paymentMode1.Name.ToString();
                        acw.CreatedDate = DateTime.UtcNow;
                        acw.UpdatedDate = DateTime.UtcNow;
                        acw.FkIdInvoiceWalletAgent = invoice.IdInvoiceWalletCashier;
                        acw.NewBalance = senderAccounting.Balance;
                        await _CatalogDbContext.AccountingOpWallets.AddAsync(acw);
                        await _CatalogDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = "No customer BALANCE";
                        rp.Code = 400;
                        transaction.Rollback();
                        return rp;

                       
                    }
                    
                    //update accounting Agent
                   
                    if (agent.Accounting != null)
                    {
                        Accounting recipientAccounting = agent.Accounting;
                        if (recipientAccounting.Balance < amountTopaid)
                        {
                            transaction.Rollback();
                            rp.Msg="Your Balance is low";
                            rp.Code = 340;
                            rp.Body = null;
                            rp.IsError=true;
                            return rp;
                        }
                            
                        recipientAccounting.Balance = recipientAccounting.Balance - amountTopaid;

                        _CatalogDbContext.Accountings.Update(recipientAccounting);
                        await _CatalogDbContext.SaveChangesAsync();
                        AccountingOpWallet aOpC = new AccountingOpWallet();
                        aOpC.IdAccountingOperation = Ulid.NewUlid();
                        aOpC.FkIdAccounting = recipientAccounting.IdAccounting;
                        aOpC.FkIdInvoiceWalletAgent = invoice.IdInvoiceWalletCashier;
                        aOpC.Credited = 0;
                        aOpC.DeBited = amountTopaid;
                        aOpC.PaymentMode = paymentMode1.Name.ToString();
                        aOpC.NewBalance = recipientAccounting.Balance;
                        aOpC.CreatedDate=DateTime.UtcNow;
                        aOpC.UpdatedDate= DateTime.UtcNow;
                        _CatalogDbContext.AccountingOpWallets.Add(aOpC);
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
                    Console.WriteLine($"Error: {ex.Message}");
                    rp.IsError = true;
                    rp.Msg = "error";
                    return rp;
                }

                rp.Body = invoice;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                rp.IsError = true;
                rp.Msg =ex.Message;
                rp.Code = 400;
                rp.Msg = "error";
            }

            //inserer operation accounting sender
            //inserer operation accounting destinataire
            return rp;
        }
        public async Task<ResponseBody<List<InvoiceWalletAgent>>> searcheInvoiceWalletAgent(string? status, string? code, DateTime? beginDate, DateTime? endDate,String? phoneAgent, String? phoneCustomer, int page, int limit)
        {
            ResponseBody<List<InvoiceWalletAgent>> rp = new ResponseBody<List<InvoiceWalletAgent>>();
            
            if (phoneAgent != null)
            {
                phoneAgent = phoneAgent.Trim().Replace(" ", "");
                
            }

            if (phoneCustomer != null)
            {
                phoneCustomer = phoneCustomer.Trim().Replace(" ", "");
                
            }

            try
            {
                DateTime myDateTime = DateTime.UtcNow;
                string dateNow = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if (endDate != null)
                {
                    dateNow = ((DateTime)endDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                string query = $"Select * from \"InvoiceWalletAgents\" ";

                query += $"where  \"CreatedDate\" <'{dateNow}' ";

                if (status != null)
                {
                    query += $"and \"InvoiceStatus\" ='{status}' ";
                }
                if (code != null)
                {
                    query += $"and  \"InvoiceCode\" ='{code}' ";
                }
                if (beginDate != null)
                {
                    string beginDateTostring = ((DateTime)beginDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    query += $"and  \"CreatedDate\" >'{beginDateTostring}' ";
                }
                //query += $";";

                Console.WriteLine(dateNow);
                Console.WriteLine(query);
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.BillerInvoices != null)
                {
                    var totalItems = 
                    (phoneAgent != null && phoneCustomer == null) ? _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Where(i => i.Agent.Phone == phoneAgent).Count() :
                    (phoneAgent != null && phoneCustomer != null) ? _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Where(i => i.Agent.Phone == phoneAgent && i.CustomerWallet.Phone == phoneCustomer).Count() :
                    (phoneAgent == null && phoneCustomer != null) ? _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Where(i => i.CustomerWallet.Phone == phoneCustomer).Count() :
                    _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Count();

                    int pageCount = (int)Math.Ceiling((decimal)totalItems / limit);
                    var ps = (phoneAgent != null && phoneCustomer==null)?await _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Include(i => i.Agent).Include(i => i.CustomerWallet).Where(i=>i.Agent.Phone==phoneAgent).ToListAsync():
                        (phoneAgent != null && phoneCustomer != null) ? await _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Include(i => i.Agent).Include(i => i.CustomerWallet).Where(i => i.Agent.Phone == phoneAgent && i.CustomerWallet.Phone==phoneCustomer).ToListAsync():
                        (phoneAgent == null && phoneCustomer != null) ? await _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Include(i => i.Agent).Include(i => i.CustomerWallet).Where(i => i.CustomerWallet.Phone == phoneCustomer).ToListAsync():
                        await _CatalogDbContext.InvoiceWalletAgents.FromSqlRaw(query).Include(i => i.Agent).Include(i => i.CustomerWallet).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;
                    }
                    else
                    {
                        rp.Body = new List<InvoiceWalletAgent>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.Message;
            }

            return rp;

        }
        public async Task<ResponseBody<List<DepositStatisticByAgentDto>>> depositStatisticByAgentDto(DateTime begenDate, DateTime endDate,string status, Ulid? idAgent)
        {
            ResponseBody<List<DepositStatisticByAgentDto>> rp = new ResponseBody<List<DepositStatisticByAgentDto>>();
            try
            {
                var groupedData = idAgent != null ? await _CatalogDbContext.InvoiceWalletAgents
               .Include(i => i.CustomerWallet)
               .Where(i => i.InvoiceStatus == status)
               .Where(i => i.Agent.Profile == "AGENT")
               .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
               .Where(i => i.FkIdAgent == idAgent)
               .GroupBy(e => new { e.FkIdAgent })
               .Select(g => new
               {
                   count=g.Count(),
                   code = g.Max(c => c.Agent.Code),
                   Phone = g.Max(c => c.Agent.Phone),
                   TotalAmount = g.Sum(e => e.AmountToPaid),
                   LastName = g.Max(c => c.Agent.LastName),
                   FirstName = g.Max(c => c.Agent.FirstName),
                   MiddleName = g.Max(c => c.Agent.MiddleName),
               }).ToArrayAsync()
               : await _CatalogDbContext.InvoiceWalletAgents
                   .Include(i => i.Agent)
                   .Where(i => i.InvoiceStatus == status)
                   .Where(i => i.Agent.Profile == "AGENT")
                   .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
                   .GroupBy(e => new { e.FkIdAgent })
                   .Select(g => new
                   {
                       count = g.Count(),
                       code = g.Max(c => c.Agent.Code),
                       Phone = g.Max(c => c.Agent.Phone),
                       TotalAmount = g.Sum(e => e.AmountToPaid),
                       LastName = g.Max(c => c.Agent.LastName),
                       FirstName = g.Max(c => c.Agent.FirstName),
                       MiddleName = g.Max(c => c.Agent.MiddleName),
                   }).ToArrayAsync();
                if (groupedData != null)
                {
                    //rp.Body = groupedData;
                    List<DepositStatisticByAgentDto> list = new List<DepositStatisticByAgentDto>();
                    foreach (var i in groupedData)
                    {
                        DepositStatisticByAgentDto b = new DepositStatisticByAgentDto();
                        b.Count=i.count;
                        b.Code = i.code;
                        b.Phone = i.Phone;
                        b.LastName = i.LastName;
                        b.FirstName = i.FirstName;
                        b.MiddleName = i.MiddleName;
                        b.TotalAmount = i.TotalAmount;
                        list.Add(b);
                    }
                    rp.Body = list;
                }
                else
                {
                    rp.Body = new List<DepositStatisticByAgentDto>();
                }
                foreach (var i in groupedData)
                {
                    Console.WriteLine(i);
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 400;
            }

            return rp;


        }

        public async Task<ResponseBody<InvoiceWalletAgent>> withdraw(BodyInvoiceWalletCashier ac)
        {
            CustomerWallet customer = null;

            CustomerWallet agent = null;

            FeeSend feeSend = null;
            ResponseBody<InvoiceWalletAgent> rp = new ResponseBody<InvoiceWalletAgent>();
            
            PaymentMode paymentMode1 = new PaymentMode();
            try
            {
                //rechercher customer
                BodyPhoneShDto bp = new BodyPhoneShDto();
                bp.CountryIdentity = ac.PhoneIdentityCustomerWallet;
                bp.Phone = ac.PhoneCustomerWallet;
                ResponseBody<CustomerWallet> senderRep = await _customerWalleServ.findCustomerWalletContryidentityAndPhone(bp);
                if (senderRep != null)
                {
                    if (!senderRep.IsError)
                    {
                        customer = senderRep.Body;
                        if (customer.Accounting.Balance <= ac.AmountToSend)
                        {
                            throw new Exception("Low balance for this transaction");
                        }
                    }
                    else
                    {
                        throw new Exception(senderRep.Msg);
                    }
                }
                
                //rechercher cashier 

                ResponseBody<CustomerWallet> caReps = await _customerWalleServ.findCustomerWalletById(ac.IdAgent);
                if (caReps != null)
                {
                    if (!caReps.IsError)
                    {
                        Console.WriteLine("mmmm");
                        agent = caReps.Body;
                        if (agent.Profile != "AGENT")
                        {
                            throw new Exception("This operation is not allowed");
                        }
                       // bcc.idCashier = recipient.FkIdAgency;
                        //.WriteLine("mmmm" + cashier.FkIdAgency);
                    }
                    else
                    {
                        throw new Exception(caReps.Msg);
                    }
                }
               
                //rechercher cashier

              
               
                //rechercher le mode de payement
                ResponseBody<PaymentMode> paymentMode = await _paymentModeServ.findByPaymentMode("WD");
                if (paymentMode != null)
                {
                    if (!paymentMode.IsError)
                    {
                        paymentMode1 = paymentMode.Body;
                       
                    }
                    else
                    {
                        throw new Exception(paymentMode.Msg);
                    }
                }
                //rechercher fee_send idCorridor & 

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
                //Add fee_payee

                //end

                InvoiceWalletAgent invoice = new InvoiceWalletAgent();

                double ConvertToUnixTimestamp(DateTime date)
                {
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan diff = date.ToUniversalTime() - origin;
                    return Math.Floor(diff.TotalSeconds);
                }
                Console.WriteLine(customer.IdCustomerWallet + "----------------------------------------------" + customer.FirstName);
                string dayToday = ConvertToUnixTimestamp(DateTime.UtcNow).ToString();
                invoice.InvoiceCode = "N" + dayToday + dayToday.Substring(1, 3);
                invoice.InvoiceCode2 = "";
                invoice.FkIdFeeSend = feeSend.IdFeeSend; 
                invoice.FkIdPaymentMode =paymentMode1.IdPaymentMode;
                invoice.PaymentMode = paymentMode1.Name.ToString();
                invoice.FkIdCustomerWallet = customer.IdCustomerWallet;
                
                invoice.AmountToSend = ac.AmountToSend;
                invoice.InvoiceStatus = "P";

                invoice.CustomerWallet = null;
                
                invoice.CreatedDate = DateTime.UtcNow;
                invoice.UpdatedDate = DateTime.UtcNow;


                //calculer fee amountToSend amountToPaid
                float aa = 0.01f;
                Console.WriteLine(aa + " " + 0.01 * 100);
                Console.WriteLine(0.01f * 100);
                decimal feeForSendPercent = new decimal(feeSend.PercentAgFee);
                double amountToSend = (double)ac.AmountToSend;
                
                double amountTopaid = ((double)feeForSendPercent * amountToSend) + amountToSend;
                Console.WriteLine(feeForSendPercent + "*" + amountToSend + "=" + amountTopaid);
                //ac.AmountToSend = (amountToSend * (double)corridor.Rate ) + ((amountToSend * (double)corridor.Rate) * (double)feeSend.PercentAgFee);
                invoice.AmountToPaid = amountTopaid;
                double amountToReceved = amountToSend;
                
                invoice.FeesAmount = (double)feeForSendPercent * amountToSend;

                Console.WriteLine(amountToReceved);
                var transaction = _CatalogDbContext.Database.BeginTransaction();
              
                    try
                {
                    //inserer tansactions
                    _CatalogDbContext.InvoiceWalletAgents.Add(invoice);
                    await _CatalogDbContext.SaveChangesAsync();

                    //update accounting sender

                    if (customer.Accounting != null)
                    {
                        Accounting senderAccounting = customer.Accounting;
                        if (senderAccounting.Balance < amountTopaid)
                        {
                            throw new Exception("Your Balance is low");
                        }
                        senderAccounting.Balance = senderAccounting.Balance - amountTopaid;
                        _CatalogDbContext.Accountings.Update(senderAccounting);
                       
                        await _CatalogDbContext.SaveChangesAsync();
                        AccountingOpWallet acw = new AccountingOpWallet();
                        acw.FkIdAccounting = senderAccounting.IdAccounting;
                        acw.Credited = 0;
                        acw.PaymentMode=paymentMode1.Name.ToString();
                        acw.DeBited = amountToSend;
                        acw.FkIdInvoiceWalletAgent = invoice.IdInvoiceWalletCashier;
                        acw.NewBalance = senderAccounting.Balance;
                        acw.CreatedDate = DateTime.UtcNow;
                        acw.UpdatedDate = DateTime.UtcNow;
                        
                        
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


                    //update accounting agent
                    
                       
                        if (agent.Accounting != null)
                        {
                            Accounting recipientAccounting = agent.Accounting;
                            recipientAccounting.Balance = recipientAccounting.Balance + (amountToSend);
                            _CatalogDbContext.Accountings.Update(recipientAccounting);
                            await _CatalogDbContext.SaveChangesAsync();
                            AccountingOpWallet aOpC = new AccountingOpWallet();
                            aOpC.FkIdAccounting = recipientAccounting.IdAccounting;
                            aOpC.FkIdInvoiceWalletAgent = invoice.IdInvoiceWalletCashier;
                            aOpC.Credited = amountToSend;
                            aOpC.PaymentMode=paymentMode1.Name.ToString();
                            aOpC.DeBited = 0;
                            aOpC.NewBalance = recipientAccounting.Balance;
                            _CatalogDbContext.AccountingOpWallets.Add(aOpC);
                            aOpC.CreatedDate = DateTime.UtcNow;
                            aOpC.UpdatedDate = DateTime.UtcNow;
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
                catch (Exception ex)
                {
                    rp.IsError = true;
                    rp.Msg = ex.ToString();
                }




                rp.Body = invoice;
            }
            catch (Exception ex)
            {

                rp.IsError = true;
                rp.Msg = ex.ToString();
            }

            //inserer operation accounting sender
            //inserer operation accounting destinataire
            return rp;
        }

        public async Task<ResponseBody<InvoiceWalletAgent>> findWalletCashierById(Ulid idInvoice)
        {
            ResponseBody<InvoiceWalletAgent> rp = new ResponseBody<InvoiceWalletAgent>();
            try
            {

                InvoiceWalletAgent ac=await _CatalogDbContext.InvoiceWalletAgents.Include(i => i.PaymentModeObj).Include(i=>i.Agent).Include(i => i.CustomerWallet).Where(i=>i.IdInvoiceWalletCashier==idInvoice).FirstOrDefaultAsync();
                
                rp.Body = ac;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Code = 400;
            }
            return rp;
        }
    }
}
