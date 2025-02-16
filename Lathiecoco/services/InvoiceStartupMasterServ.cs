using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace  Lathiecoco.services
{
    public class InvoiceStartupMasterServ : InvoiceStartupMasterRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly IConfiguration _configuration;
        public InvoiceStartupMasterServ(CatalogDbContext CatalogDbContext, IConfiguration configuration)
        {
            _CatalogDbContext = CatalogDbContext;
            _configuration = configuration;
        }
        public string bucketName = "uploads-proof1";
        public async Task<ResponseBody<List<InvoiceStartupMaster>>> findAllInvoiceStartupMasterAgency(int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceStartupMasters != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.InvoiceStartupMasters.Count() / limit);
                    var ps = await _CatalogDbContext.InvoiceStartupMasters
                        .Include(c => c.AgencyUser)
                        .Include(c => c.AgencyUser.Agency)
                        .Include(i => i.Staff)
                        .Include(i => i.Agent)
                        .OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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
                rp.Code = 400;
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
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.Staff).Include(i => i.Agent).Where(i=>i.FkIdAgent==fkIdMaster).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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
                rp.Code = 400;
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
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.Include(i=>i.Staff).Include(i => i.Agent).Where(i => i.FkIdStaff == fkIdOwnerAgent).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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
                rp.Code = 400;

            }
            return rp;
        }

        public async Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByAgency(Ulid idAgency, int page = 1, int limit = 10)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.InvoiceStartupMasters != null)
                {
                    var req = _CatalogDbContext.InvoiceStartupMasters.Include(i => i.AgencyUser).Include(i => i.Agent).Where(i => i.Agent.FkIdAgency == idAgency || i.AgencyUser.FkIdAgency==idAgency);
                    double totalCount = req.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
                    var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;
                        rp.TotalCount = totalCount;

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
                rp.Code = 400;

            }
            return rp;
        }

        public async Task<ResponseBody<string>>getFileUrl(string key)
        {

            ResponseBody<string> rp = new ResponseBody<string>();

            try
            {
                Console.WriteLine(_configuration.GetValue<string>("AWS:AccessKey"));
                string _accessKey = _configuration.GetValue<string>("AWS:AccessKey");
                
                string _secretKey = _configuration.GetValue<string>("AWS:SecretKey");
                string _region = _configuration.GetValue<string>("AWS:Region");

                var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                var region = RegionEndpoint.GetBySystemName(_region);
                var client = new AmazonS3Client(credentials, region);
                Console.WriteLine(client);

                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
                if (!bucketExists)
                {
                    rp.Msg = "Bucket not exist";
                    rp.IsError = true;
                    return rp;
                }
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                string url= await client.GetPreSignedURLAsync(urlRequest);
                rp.Body = url;
                

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.ToString();
                return rp;
            }

            return rp;
        }
        public async Task<ResponseBody<InvoiceStartupMaster>> uploadProof(IFormFile formFile, Ulid idInvoiceStartupMaster)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            try
            {

                if (formFile != null)
                {
                    string _accessKey = _configuration.GetValue<string>("AWS:AccessKey");
                    string _secretKey = _configuration.GetValue<string>("AWS:SecretKey");
                    string _region = _configuration.GetValue<string>("AWS:Region");
                    var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                    var region = RegionEndpoint.GetBySystemName(_region);
                    var client =new AmazonS3Client(credentials, region);

                    var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
                    if (!bucketExists) {
                        rp.Msg = "Bucket not exist";
                        rp.IsError=true;
                        return rp;
                    }
                    var request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key =  formFile.FileName,
                        InputStream = formFile.OpenReadStream()
                    };
                    request.Metadata.Add("Content-Type", formFile.ContentType);
                    await client.PutObjectAsync(request);
                    
                    
                    
                    InvoiceStartupMaster ism = await _CatalogDbContext.InvoiceStartupMasters.Where(i => i.IdInvoiceStartupMaster == idInvoiceStartupMaster).FirstOrDefaultAsync();
                    if (ism != null)
                    {
                        //string path=Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                        //using FileStream stream = new FileStream(Path.Combine(path,formFile.FileName), FileMode.Create);
                        //formFile.CopyTo(stream);
                            
                        
                        ism.ProofLink = formFile.FileName;
                        _CatalogDbContext.InvoiceStartupMasters.Update(ism);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = ism;
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Code = 460;
                        rp.Msg = "Transaction not found";
                        return rp;
                    }

                }
            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.Message;
                return rp;
            }
           


            return rp;
        }

        public async Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMaster(BodyInvoiceStartupMaster ism)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            InvoiceStartupMaster invoice = new InvoiceStartupMaster();
            var transaction = _CatalogDbContext.Database.BeginTransaction();

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

                        if (!master1.IsActive)
                        {
                            rp.IsError = true;
                            rp.Body = null;
                            rp.Msg = "You account is not active";
                            return rp;
                        }

                        if (master1.IsBlocked)
                        {
                            rp.IsError = true;
                            rp.Body = null;
                            rp.Msg = "Your account is blocked";
                            return rp;
                        }
                    if (master1.PercentagePurchase == null)
                    {
                        rp.IsError = true;
                        rp.Body = null;
                        rp.Code = 450;
                        rp.Msg = "Percentage Purchase not defined!!";
                        return rp;
                    }
                    else
                    {
                        invoice.AmountToPaid = ((double)Math.Ceiling((decimal)(master1.PercentagePurchase * ism.AmountToSend))+ ism.AmountToSend);
                    }

                        invoice.IdInvoiceStartupMaster= Ulid.NewUlid();
                        invoice.InvoiceStatus = "A";
                        invoice.AmountToSend = ism.AmountToSend;

                        //invoice.AmountToReceived = ism.AmountToSend;
                        //invoice.FkIdStaff = ism.FkIdStaff;
                        invoice.FkIdAgent = ism.FkIdMaster;
                        invoice.CreatedDate = DateTime.UtcNow;
                        invoice.UpdatedDate = DateTime.UtcNow;
                        
                        string dayToday =GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                        invoice.InvoiceCode = "M" + dayToday;
                        //searche paymentMode
                        PaymentMode pm = await _CatalogDbContext.PaymentModes.Where(o => o.Name == "AM").FirstOrDefaultAsync();

                        if (pm != null)
                        {
                            FeeSend feeSend =await _CatalogDbContext.FeeSends.Where(f=>f.FkIdPaymentMode==pm.IdPaymentMode).FirstOrDefaultAsync();

                        if (feeSend != null)
                        {
                            if (feeSend.MinAmount < ism.AmountToSend && feeSend.MaxAmount > ism.AmountToSend)
                            {

                            }
                            else
                            {
                                rp.Msg = "Amount is not in defined interval!!";
                                rp.IsError = true;
                                rp.Code = 305;
                                return rp;
                            }
                        }
                        else
                        {
                            rp.Msg = "fees not configured: " + pm.Name + "!"; ;
                            rp.IsError = true;
                            rp.Code = 602;
                            return rp;
                        }
                            
                            
                        
                        invoice.FkIdPaymentMode = pm.IdPaymentMode;
                            invoice.PaymentMode = pm.Name.ToString();
                            _CatalogDbContext.InvoiceStartupMasters.Add(invoice);
                            await _CatalogDbContext.SaveChangesAsync();
                            rp.Body = invoice;
                        AccountingOpWallet aOpCsh = new AccountingOpWallet();
                        aOpCsh.IdAccountingOperation = Ulid.NewUlid();
                        aOpCsh.FkIdAccounting = master1.FkIdAccounting;
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpCsh.Credited = 0;
                        aOpCsh.DeBited = 0;
                        aOpCsh.PaymentMode = invoice.PaymentModeObj.Name.ToString();
                        aOpCsh.NewBalance = 0;
                        aOpCsh.CreatedDate = DateTime.UtcNow;
                        aOpCsh.UpdatedDate = DateTime.UtcNow;
                        _CatalogDbContext.AccountingOpWallets.Add(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                        else
                        {
                            rp.Msg = "Payment mode MA not found!";
                            rp.IsError = true;
                            rp.Code = 600;
                            return rp;
                        
                        }
                    }
                    else
                    {
                        rp.Msg = "Master not found!";
                        rp.IsError = true;
                        rp.Code = 601;
                        return rp;
                        
                    }
                


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                rp.IsError = true;
                rp.Msg = ex.Message;
            }

            return rp;
        }
        public async Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMasterByAgency(BodyInvoiceStartupMasterByAgent ism)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            InvoiceStartupMaster invoice = new InvoiceStartupMaster();
            var transaction = _CatalogDbContext.Database.BeginTransaction();

            try
            {
                //verifyAgent
                //OwnerAgent staff = await _CatalogDbContext.OwnerAgents.Where(c => c.IdOwnerAgent == ism.FkIdStaff ).FirstOrDefaultAsync();
                Agency agency = null;
                AgencyUser master1 = await _CatalogDbContext.AgencyUsers.Include(a=>a.Agency).Include(a => a.Agency.Accounting).Where(c => c.IdAgencyUser == ism.FkIdAgencyUser).FirstOrDefaultAsync();
                if (master1 != null)
                {

                    if (!master1.IsActive)
                    {
                        rp.IsError = true;
                        rp.Body = null;
                        rp.Msg = "You account is not active";
                        return rp;
                    }

                    if (master1.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Body = null;
                        rp.Msg = "Your account is blocked!";
                        return rp;
                    }

                    agency = master1.Agency;

                    if (agency.PercentagePurchase == null)
                    {
                        rp.IsError = true;
                        rp.Body = null;
                        rp.Code = 450;
                        rp.Msg = "Percentage Purchase not defined!!";
                        return rp;
                    }
                    else
                    {
                        invoice.AmountToPaid = ((double)Math.Ceiling(((decimal)(agency.PercentagePurchase * ism.AmountToSend))) + ism.AmountToSend);
                    }

                    invoice.IdInvoiceStartupMaster = Ulid.NewUlid();
                    invoice.InvoiceStatus = "A";
                    invoice.AmountToSend = ism.AmountToSend;
                    //invoice.AmountToReceived = ism.AmountToSend;
                    //invoice.FkIdStaff = ism.FkIdStaff;
                    invoice.FkIdAgencyUser = ism.FkIdAgencyUser;
                    invoice.CreatedDate = DateTime.UtcNow;
                    invoice.UpdatedDate = DateTime.UtcNow;

                    string dayToday = GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                    invoice.InvoiceCode = "M" + dayToday;
                    //searche paymentMode
                    PaymentMode pm = await _CatalogDbContext.PaymentModes.Where(o => o.Name  == "SM").FirstOrDefaultAsync();

                    if (pm != null)
                    {
                        FeeSend feeSend = await _CatalogDbContext.FeeSends.Where(f => f.FkIdPaymentMode == pm.IdPaymentMode).FirstOrDefaultAsync();

                        if (feeSend != null)
                        {

                            if (feeSend.MinAmount < ism.AmountToSend && feeSend.MaxAmount > ism.AmountToSend)
                            {

                            }
                            else
                            {
                                rp.Msg = "Amount is not in defined interval!!";
                                rp.IsError = true;
                                rp.Code = 305;
                                return rp;
                            }
                        }
                        else
                        {
                            rp.Msg = "fees not configured: " + pm.Name +"!";
                            rp.IsError = true;
                            rp.Code = 602;
                            return rp;
                        }
                        invoice.FkIdPaymentMode = pm.IdPaymentMode;
                        invoice.PaymentMode = pm.Name.ToString();
                        _CatalogDbContext.InvoiceStartupMasters.Add(invoice);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = invoice;
                        AccountingOpWallet aOpCsh = new AccountingOpWallet();
                        aOpCsh.IdAccountingOperation = Ulid.NewUlid();
                        if (agency != null)
                        {
                            aOpCsh.FkIdAccounting = (Ulid)agency.FkIdAccounting;
                        }
                       
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpCsh.Credited = 0;
                        aOpCsh.DeBited = 0;
                        aOpCsh.PaymentMode = invoice.PaymentModeObj.Name.ToString();
                        aOpCsh.NewBalance = 0;
                        aOpCsh.CreatedDate = DateTime.UtcNow;
                        aOpCsh.UpdatedDate = DateTime.UtcNow;
                        _CatalogDbContext.AccountingOpWallets.Add(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    else
                    {
                        rp.Msg = "Payment mode SM not found!";
                        rp.IsError = true;
                        rp.Code = 600;
                        return rp;
                    }
                }
                else
                {
                    rp.Msg = "Master not found!";
                    rp.IsError = true;
                    rp.Code = 601;
                    return rp;
                  
                }



            }
            catch (Exception ex)
            {
                transaction.Rollback();
                rp.IsError = true;
                rp.Msg = ex.Message;
            }

            return rp;
        }

        public async Task<ResponseBody<InvoiceStartupMaster>> findInvoiceStarupMasterById(Ulid id)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();
            try
            {

                InvoiceStartupMaster ism = await _CatalogDbContext.InvoiceStartupMasters
                    .Include(c => c.AgencyUser)
                    .Include(c => c.AgencyUser.Agency)
                    .Include(c => c.PaymentModeObj)
                    .Include(c => c.Agent).Include(c => c.Staff)
                    .Where(c => c.IdInvoiceStartupMaster == id).FirstOrDefaultAsync();
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
        public async Task<ResponseBody<List<InvoiceStartupMaster>>> searcheInvoiceStartupMaster(string? status, string? code, DateTime? beginDate, DateTime? endDate, int page, int limit)
        {
            ResponseBody<List<InvoiceStartupMaster>> rp = new ResponseBody<List<InvoiceStartupMaster>>();
            try
            {
                DateTime myDateTime = DateTime.UtcNow;
                string dateNow = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if (endDate != null)
                {
                    dateNow = ((DateTime)endDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                string query = $"Select * from InvoiceStartupMasters ";

                query += $"where CreatedDate<'{dateNow}' ";

                if (status != null)
                {
                    query += $"and InvoiceStatus='{status}' ";
                }
                if (code != null)
                {
                    query += $"and  InvoiceCode='{code}' ";
                }
                if (beginDate != null)
                {
                    string beginDateTostring = ((DateTime)beginDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    query += $"and  CreatedDate>'{beginDateTostring}' ";
                }
                query += $";";

                Console.WriteLine(dateNow);
                Console.WriteLine(query);
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.BillerInvoices != null)
                {
                    var totalItems = _CatalogDbContext.BillerInvoices.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalItems / limit);
                    var ps = await _CatalogDbContext.InvoiceStartupMasters.FromSqlRaw(query).ToListAsync();
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
                Console.WriteLine(ex.ToString());
                rp.IsError = true;
                rp.Code = 400;
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
                    if (invoice.InvoiceStatus != "A")
                    {
                        rp.IsError = true;
                        rp.Msg = "Transaction already  validated";
                        rp.Code = 505;
                        rp.Body = null;
                        return rp;
                    }
                    try
                    {

                        //get Master 
                        CustomerWallet Master = invoice.Agent;
                        if (Master != null)
                        {


                            MasterAccounting = Master.Accounting;
                            if (MasterAccounting != null)
                            {

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
                        AccountingOpWallet aOpCsh = await _CatalogDbContext.AccountingOpWallets.Where(i => i.FkIdInvoiceStartupMaster == bodyValidInvoiceStartupMaster.IdInvoiceStartupMaster && i.FkIdAccounting== MasterAccounting.IdAccounting).FirstOrDefaultAsync();

                        //aOpCsh.FkIdAccounting = MasterAccounting.IdAccounting;
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpCsh.Credited =invoice.AmountToPaid;
                        aOpCsh.DeBited = 0;
                        aOpCsh.NewBalance = MasterAccounting.Balance;

                        aOpCsh.UpdatedDate = DateTime.UtcNow;
                        _CatalogDbContext.AccountingOpWallets.Update(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();

                        //Change status of transactions
                        invoice.InvoiceStatus = "P";
                        invoice.ValidateAt = DateTime.UtcNow;
                        invoice.UpdatedDate = DateTime.UtcNow;
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

        public async Task<ResponseBody<InvoiceStartupMaster>> validateInvoiceStartupMasterOfAgency(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();

            try
            {
                InvoiceStartupMaster invoice = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.PaymentModeObj).Include(i => i.Staff).Include(i => i.AgencyUser).Include(i => i.AgencyUser.Agency).Include(i => i.AgencyUser.Agency.Accounting).Where(i => i.IdInvoiceStartupMaster == bodyValidInvoiceStartupMaster.IdInvoiceStartupMaster).FirstOrDefaultAsync();
                if (invoice != null)
                {
                    var transaction = _CatalogDbContext.Database.BeginTransaction();
                    Accounting MasterAccounting = null;
                    if (invoice.InvoiceStatus != "A")
                    {
                        rp.IsError = true;
                        rp.Msg = "Transaction already  validated";
                        rp.Code = 505;
                        rp.Body = null;
                        return rp;
                    }
                    try
                    {

                        //get Master 
                        AgencyUser  agencyUser= invoice.AgencyUser;
                        if (agencyUser != null)
                        {


                            MasterAccounting = agencyUser.Agency.Accounting;
                            if (MasterAccounting != null)
                            {

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
                            throw new Exception("Agency user not found");
                            transaction.Rollback();
                        }


                        //Add opAccounting Cashier
                        AccountingOpWallet aOpCsh = await _CatalogDbContext.AccountingOpWallets.Where(i => i.FkIdInvoiceStartupMaster == bodyValidInvoiceStartupMaster.IdInvoiceStartupMaster && i.FkIdAccounting == MasterAccounting.IdAccounting).FirstOrDefaultAsync();

                        //aOpCsh.FkIdAccounting = MasterAccounting.IdAccounting;
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpCsh.Credited = invoice.AmountToPaid;
                        aOpCsh.DeBited = 0;
                        aOpCsh.NewBalance = MasterAccounting.Balance;

                        aOpCsh.UpdatedDate = DateTime.UtcNow;
                        _CatalogDbContext.AccountingOpWallets.Update(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();

                        //Change status of transactions
                        invoice.InvoiceStatus = "P";
                        invoice.ValidateAt = DateTime.UtcNow;
                        invoice.UpdatedDate = DateTime.UtcNow;
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

        public async Task<ResponseBody<InvoiceStartupMaster>> ValidateInvoiceStartupMasterByAgencyUser(ValidateInvoiceStartupMasterDto dto)
        {
            ResponseBody<InvoiceStartupMaster> rp = new ResponseBody<InvoiceStartupMaster>();

            try
            {
                InvoiceStartupMaster invoice = await _CatalogDbContext.InvoiceStartupMasters.Include(i => i.PaymentModeObj).Include(i => i.Staff).Include(i => i.Agent).Include(i => i.Agent.Accounting).Where(i => i.IdInvoiceStartupMaster == dto.IdInvoiceStartupMaster).FirstOrDefaultAsync();
                if (invoice != null)
                {
                    var transaction = _CatalogDbContext.Database.BeginTransaction();
                    Accounting AgentAccounting = null;
                    Accounting MasterAccounting = null;
                    AgencyUser AgencyUser = null;

                    if (invoice.InvoiceStatus != "A")
                    {
                        rp.IsError = true;
                        rp.Msg = "Transaction already validated";
                        rp.Code = 505;
                        rp.Body = null;
                        return rp;
                    }

                    try
                    {
                        AgencyUser = await _CatalogDbContext.AgencyUsers.Include(a=>a.Agency).Include(a => a.Agency.Accounting).Where(a=>a.IdAgencyUser==dto.IdAgencyUser).FirstOrDefaultAsync();
                        if( AgencyUser != null )
                        {
                            if (AgencyUser.Agency != null)
                            {
                                Agency agency = AgencyUser.Agency;

                                MasterAccounting = agency.Accounting;
                                if (MasterAccounting != null)
                                {
                                    if (invoice.AmountToPaid > MasterAccounting.Balance)
                                    {
                                        rp.IsError = true;
                                        rp.Msg = "The amount in your account is insufficient";
                                        rp.Code = 340;
                                        rp.Body = null;
                                        return rp;
                                    }

                                    MasterAccounting.Balance = MasterAccounting.Balance - invoice.AmountToPaid;
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
                                transaction.Rollback();
                                rp.IsError = true;
                                rp.Msg = "agency of user not found";
                                rp.Code = 421;
                                rp.Body = null;
                                return rp;
                            }
                        }else
                        {
                            transaction.Rollback();
                            rp.IsError = true;
                            rp.Msg = "User of agency not found";
                            rp.Code = 420;
                            rp.Body = null;
                            return rp;
                        }

                        //get Master 
                        CustomerWallet Master = invoice.Agent;
                        if (Master != null)
                        {
                            //Have a company
                            if(Master.FkIdAgency!=null)
                            {
                                //Same company
                                if (AgencyUser.FkIdAgency != Master.FkIdAgency)
                                {
                                    rp.IsError = true;
                                    rp.Msg = "You cannot validate a transaction from another company!!";
                                    rp.Code = 506;
                                    rp.Body = null;
                                    return rp;
                                }
                            }
                            else
                            {
                                rp.IsError = true;
                                rp.Msg = "Agent not have a company!!";
                                rp.Code = 507;
                                rp.Body = null;
                                return rp;
                            }

                            AgentAccounting = Master.Accounting;
                            if (AgentAccounting != null)
                            {

                            AgentAccounting.Balance = AgentAccounting.Balance + invoice.AmountToPaid;
                                _CatalogDbContext.Accountings.Update(AgentAccounting);
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

                        
                        AccountingOpWallet aOpMasterAccounting = new AccountingOpWallet();
                        aOpMasterAccounting.IdAccountingOperation=Ulid.NewUlid();
                        aOpMasterAccounting.FkIdAccounting = MasterAccounting.IdAccounting;
                        aOpMasterAccounting.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        aOpMasterAccounting.NewBalance = MasterAccounting.Balance;
                        aOpMasterAccounting.PaymentMode = invoice.PaymentMode;
                        aOpMasterAccounting.DeBited = invoice.AmountToPaid;
                        aOpMasterAccounting.CreatedDate= DateTime.UtcNow;
                        aOpMasterAccounting.Credited = 0;

                        aOpMasterAccounting.UpdatedDate = DateTime.UtcNow;
                        await _CatalogDbContext.AccountingOpWallets.AddAsync(aOpMasterAccounting);
                        await _CatalogDbContext.SaveChangesAsync();

                        //Add opAccounting Cashier
                        AccountingOpWallet aOpCsh = await _CatalogDbContext.AccountingOpWallets.Where(i=>i.FkIdInvoiceStartupMaster== dto.IdInvoiceStartupMaster && i.FkIdAccounting== AgentAccounting.IdAccounting).FirstOrDefaultAsync();
                        aOpCsh.FkIdInvoiceStartupMaster = invoice.IdInvoiceStartupMaster;
                        //aOpCsh.FkIdAccounting = AgentAccounting.IdAccounting;
                        aOpCsh.NewBalance = AgentAccounting.Balance;
                        aOpCsh.Credited = invoice.AmountToPaid;
                        aOpCsh.UpdatedDate = DateTime.UtcNow;
                        aOpCsh.DeBited = 0;
                        
                        _CatalogDbContext.AccountingOpWallets.Update(aOpCsh);
                        await _CatalogDbContext.SaveChangesAsync();

                        //Change status of transactions
                        invoice.InvoiceStatus = "P";
                        invoice.ValidateAt=DateTime.UtcNow;
                        invoice.UpdatedDate = DateTime.UtcNow;
                        invoice.FkIdAgencyUser = dto.IdAgencyUser;
                        _CatalogDbContext.InvoiceStartupMasters.Update(invoice);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = invoice;
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        rp.IsError = true;
                        rp.Msg = ex.ToString();
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
                rp.Msg = ex.ToString();
                

            }
            return rp;
        }
    }
}
