using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace  Lathiecoco.services
{
    public class InvoiceStartupMasterServ : InvoiceStartupMasterRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public InvoiceStartupMasterServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<List<InvoiceStartupMaster>>> findAllInvoiceStartupMasterAgency(int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceStartupMasters != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceStartupMasters.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.Staff).Include(i => i.Agent).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<InvoiceStartupMaster>();
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

        public async Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByMaster(Ulid fkIdMaster, int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceStartupMasters != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceStartupMasters.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.Staff).Include(i => i.Agent).Where(i=>i.FkIdAgent==fkIdMaster).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<InvoiceStartupMaster>();
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

        public async Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByOwnerAgent(Ulid fkIdOwnerAgent, int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceStartupMasters != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceStartupMasters.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.Include(i=>i.Staff).Include(i => i.Agent).Where(i => i.FkIdStaff == fkIdOwnerAgent).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<InvoiceStartupMaster>();
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

        public async Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMaster(BodyInvoiceStartupMaster ism)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            InvoiceStartupMaster invoice = new InvoiceStartupMaster();



            try
            {
                //verifyAgent
                //OwnerAgent staff = await _CatalogDbContext.OwnerAgents.Where(c => c.IdOwnerAgent == ism.FkIdStaff ).FirstOrDefaultAsync();
               
                    CustomerWallet master1 = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == ism.FkIdMaster).FirstOrDefaultAsync();
                    if (master1 != null)
                    {
                        if(master1.Profile != "AGENT")
                        {
                            throw new Exception("This operation is not allowed");
                        }
                        invoice.IdInvoiceStartupMaster= Ulid.NewUlid();
                        invoice.InvoiceStatus = "A";
                        invoice.AmountToSend = ism.AmountToSend;
                        invoice.AmountToPaid = ism.AmountToSend;
                        invoice.AmountToReceived = ism.AmountToSend;
                        //invoice.FkIdStaff = ism.FkIdStaff;
                        invoice.FkIdAgent = ism.FkIdMaster;
                        invoice.CreatedDate = DateTime.Now;
                        invoice.UpdatedDate = DateTime.Now;
                        double ConvertToUnixTimestamp(DateTime date)
                        {
                            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            TimeSpan diff = date.ToUniversalTime() - origin;
                            return Math.Floor(diff.TotalSeconds);
                        }
                        string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
                        invoice.InvoiceCode = "M" + dayToday + dayToday.Substring(1, 3);
                        //searche paymentMode
                        PaymentMode pm = await _CatalogDbContext.PaymentModes.Where(o => o.Name.ToString() == "SM").FirstOrDefaultAsync();

                        if (pm != null)
                        {
                            invoice.FkIdPaymentMode = pm.IdPaymentMode;
                            invoice.PaymentMode = pm.Name.ToString();
                            _CatalogDbContext.InvoiceStartupMasters.Add(invoice);
                            await _CatalogDbContext.SaveChangesAsync();
                            rp.Body = invoice;
                        }
                        else
                        {
                            throw new Exception("Payment mode MA not found");
                        }
                    }
                    else
                    {
                        throw new Exception("Master not found");
                    }
                


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }

            return rp;
        }

        public async Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMasterById(Ulid id)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            try
            {

                InvoiceStartupMaster ism = await _CatalogDbContext.InvoiceStartupMasters.Include(c => c.PaymentModeObj).Include(c => c.Agent).Include(c => c.Staff).Where(c => c.IdInvoiceStartupMaster == id).FirstOrDefaultAsync();
                if (ism != null)
                {
                    rp.Body = ism;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Invoice "+id+" not found";
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<InvoiceStartupMaster>> ValidateInvoiceStartupMaster(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();

            try
            {
                InvoiceStartupMaster invoice = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.PaymentModeObj).Include(i => i.Staff).Include(i => i.Agent).Include(i => i.Agent.Accounting).Where(i => i.IdInvoiceStartupMaster == bodyValidInvoiceStartupMaster.IdInvoiceStartupMaster).FirstOrDefaultAsync();
                if (invoice != null)
                {
                    var transaction = _CatalogDbContext.Database.BeginTransaction();
                    Accounting MasterAccounting = null;
                    //AccountingPrincipal StaffAccounting = null;
                    try
                    {
                        //Update Master Agency Accounting
                        /**
                       OwnerAgent staff = invoice.Staff;
                       if (staff != null)
                       {
                           
                               
                            if (compagny!=null) {
                                StaffAccounting = compagny.AccountingPrincipals;
                                if (StaffAccounting != null)
                                {
                                    if (StaffAccounting.Balance < invoice.AmountToPaid)
                                    {
                                        throw new Exception("Your balance is low for this transactions");
                                    }
                                    StaffAccounting.Balance = StaffAccounting.Balance - invoice.AmountToPaid;
                                    _CatalogDbContext.AccountingPrincipals.Update(StaffAccounting);
                                    await _CatalogDbContext.SaveChangesAsync();

                                }
                                else
                                {
                                    throw new Exception("Invoice Have not Staff  Accounting");
                                }
                    }


                        else
                        {
                            throw new Exception("Invoice Have not Staff");
                        }
                        


                        
                        AccountingOpPrincipal aOpStaff = new AccountingOpPrincipal();
                        aOpStaff.FkIdAccounting = StaffAccounting.IdAccountingPrincipal;
                        aOpStaff.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpStaff.Credited = 0;
                        aOpStaff.DeBited = invoice.AmountToPaid;
                        aOpStaff.PaymentMode = invoice.PaymentModeObj.Name;
                        aOpStaff.NewBalance = StaffAccounting.Balance;
                        aOpStaff.CreatedDate = DateTime.Now;
                        aOpStaff.UpdatedDate = DateTime.Now;
                        _CatalogDbContext.AccountingOpPrincipals.Add(aOpStaff);
                        await _CatalogDbContext.SaveChangesAsync();
                        **/
                        //update Master Agency Accounting
                        CustomerWallet Master = invoice.Agent;
                        if (Master != null)
                        {
                               
                            
                                MasterAccounting = Master.Accounting;
                                if (MasterAccounting != null)
                                {
                                    //Voir si le master et le cashier ont le meme currency
                                    if (MasterAccounting.Currency != MasterAccounting.Currency)
                                    {
                                        throw new Exception("Currency is not same");
                                    }
                                    MasterAccounting.Balance = MasterAccounting.Balance + invoice.AmountToPaid;
                                    _CatalogDbContext.Accountings.Update(MasterAccounting);
                                    await _CatalogDbContext.SaveChangesAsync();

                                }
                                else
                                {
                                    throw new Exception("Invoice Have not Master Agency Accounting");
                                    transaction.Rollback();
                                }


                            
                           

                        }
                        else
                        {
                            throw new Exception("Invoice Have not master");
                            transaction.Rollback();
                        }


                        //Add opAccounting Cashier
                        AccountingOpWallet aOpCsh = new AccountingOpWallet();
                        aOpCsh.IdAccountingOperation= Ulid.NewUlid();
                        aOpCsh.FkIdAccounting = MasterAccounting.IdAccounting;
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpCsh.Credited = invoice.AmountToPaid;
                        aOpCsh.DeBited = 0;
                        aOpCsh.PaymentMode = invoice.PaymentModeObj.Name.ToString();
                        aOpCsh.NewBalance = MasterAccounting.Balance;
                        aOpCsh.CreatedDate = DateTime.Now;
                        aOpCsh.UpdatedDate = DateTime.Now;
                        _CatalogDbContext.AccountingOpWallets.Add(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();

                        //Change status of transactions
                        invoice.InvoiceStatus = "P";
                        invoice.FkIdStaff = bodyValidInvoiceStartupMaster.FkIdStaff;
                        _CatalogDbContext.InvoiceStartupMasters.Update(invoice);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = invoice;
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        rp.IsError = true;
                        rp.Msg = ex.Message;
                        rp.Body = null;
                    }

                }
                else
                {
                    throw new Exception("Invoice Not found");
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
