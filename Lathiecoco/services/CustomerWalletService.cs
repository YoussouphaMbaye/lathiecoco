using Amazon;
using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace  Lathiecoco.services
{
    public class CustomerWalletService : CustomerWalletRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public CustomerWalletService(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }

        public async Task<ResponseBody<CustomerWallet>> definePercentagePurchase(DefinePercentagePurchaseMasterDto dto)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == dto.IdCustomer).FirstOrDefaultAsync();
                if (cus != null)
                {

                    cus.PercentagePurchase=dto.Percentage;
                    cus.UpdatedDate = DateTime.UtcNow;
                    cus.FkIdAgencyUser=dto.IdAgencyUser;
                    
                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
                    rp.Code = 350;
                    return rp;
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

        public async Task<ResponseBody<CustomerWallet>> activateWallet(ActiveBlockDto dto)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == dto.IdUser).FirstOrDefaultAsync();
                if (cus != null)
                {
                    
                     cus.IsActive = !cus.IsActive;
                     cus.FkIdStaff=dto.FkIdStaff;
                     cus.UpdatedDate = DateTime.UtcNow;
                    _CatalogDbContext.CustomerWallets.Update(cus);
                     await _CatalogDbContext.SaveChangesAsync();
                     rp.Body = cus;
                     return rp;
                    
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
                    rp.Code = 350;
                    return rp;
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
        public async Task<ResponseBody<CustomerWallet>> CustomerToAgencyDto(CustomerToAgencyDto dto)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == dto.IdCustomerWallet).FirstOrDefaultAsync();
                if (cus != null)
                {

                    
                    cus.FkIdStaff = dto.IdStaff;
                    cus.FkIdAgency= dto.IdAgency;
                    cus.UpdatedDate = DateTime.UtcNow;
                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
                    rp.Code = 350;
                    return rp;
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
        public async Task<ResponseBody<CustomerWallet>> blokeOrDeblokeWallet(ActiveBlockDto dto)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == dto.IdUser).FirstOrDefaultAsync();
                if (cus != null)
                {

                    cus.IsBlocked = !cus.IsBlocked;
                    cus.FkIdStaff = dto.FkIdStaff;
                    cus.UpdatedDate=DateTime.UtcNow;
                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
                    rp.Code = 350;
                    return rp;
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

        public async Task<ResponseBody<CustomerWallet>> addCustomer(CustomerWallet cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {

                await _CatalogDbContext.CustomerWallets.AddAsync(cus);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = cus;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;

        }

        public async Task<ResponseBody<CustomerWallet>> addCustomerWithAccounting(BodyCustomerWalletDto cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet c = new CustomerWallet();
                c.IdCustomerWallet= Ulid.NewUlid();
                c.FirstName = cus.FirstName;
                c.LastName = cus.LastName;
                c.MiddleName = cus.MiddleName;
                c.Address = cus.Address;
                c.Phone = cus.Phone;
                c.phoneIdentity = cus.PhoneIdentity;
                c.PinNumber = cus.PhonePinNumber;
                c.PhoneBrand= cus.PhoneBrand;
                c.Profile=cus.Profile.ToUpper();

                //should change
                Random rdn = new Random();
                var a = rdn.Next(1000, 9999);
                c.PinTemp = a.ToString();
                c.FirstName = cus.FirstName;
                c.Code = GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);

                c.CreatedDate=DateTime.UtcNow;
                c.UpdatedDate=DateTime.UtcNow;
              
                
                Accounting ac = new Accounting();
                //ac.Currency = cp.Country.CurrencyName;
                ac.IdAccounting= Ulid.NewUlid();
                ac.Balance = 0;
                ac.CreatedDate=DateTime.UtcNow;
                ac.UpdatedDate=DateTime.UtcNow;
                ac.Currency = "GNF";
                var transaction = _CatalogDbContext.Database.BeginTransaction();
                try
                {
                    await _CatalogDbContext.Accountings.AddAsync(ac);
                    await _CatalogDbContext.SaveChangesAsync();
                    c.FkIdAccounting = ac.IdAccounting;

                    await _CatalogDbContext.CustomerWallets.AddAsync(c);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = c;
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
               

            }
            catch (Exception ex)
            {
               
                rp.Msg = ex.ToString();
                rp.Code = 400;
                rp.IsError = true;
                rp.Body = null;
            }
            return rp;
        }

        public async Task<ResponseBody<CustomerWallet>> addCustomerWithAccountingPhoneOnly(BodyCustomerPhoneDto cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cusExist =await  _CatalogDbContext.CustomerWallets.Where(x => x.Phone == cus.Phone).FirstOrDefaultAsync();
                if(cusExist != null)
                {
                    rp.Msg = "Customer Already Exist!";
                    rp.IsError = true;
                    rp.Code = 350;
                    rp.Body = cusExist;
                    return rp;
                }
                CustomerWallet c = new CustomerWallet();
               
                c.IdCustomerWallet= Ulid.NewUlid();
                c.phoneIdentity = cus.CountryPhoneIdentity;
                c.Phone = cus.Phone;
                c.FirstName = "";
                c.LastName = "";
                c.MiddleName = "";
                c.Address = "";
                c.PinNumber = "";
                c.Profile = "CUSTOMER";
                c.PhoneBrand = "";
                c.IsActive = false;
                
                Random rdn = new Random();
                var a = rdn.Next(1000, 9999);
                //SMS c.PinTemp = a.ToString();
                c.PinTemp = "1234";
                string dayToday = GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                c.Code = "C" + dayToday;
                c.CreatedDate = DateTime.UtcNow;
                c.UpdatedDate = DateTime.UtcNow;

                Accounting ac = new Accounting();
                ac.IdAccounting = Ulid.NewUlid();
                ac.Currency = "GNF";
                ac.Balance = 0;
                ac.CreatedDate = DateTime.UtcNow;
                ac.UpdatedDate = DateTime.UtcNow;
                var transaction = _CatalogDbContext.Database.BeginTransaction();
                try
                {
                    await _CatalogDbContext.Accountings.AddAsync(ac);
                    await _CatalogDbContext.SaveChangesAsync();
                    c.FkIdAccounting = ac.IdAccounting;

                    await _CatalogDbContext.CustomerWallets.AddAsync(c);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = c;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                   
                }


            }
            catch (Exception ex)
            {
                rp.Msg = ex.ToString();
                rp.IsError = true;
                rp.Body = null;
                rp.Code = 400;
            }
            return rp;
        }

        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(int page = 1, int limit = 10)
        {
            ResponseBody<List<CustomerWallet>> rp = new ResponseBody<List<CustomerWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.CustomerWallets != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.CustomerWallets.Count() / limit);
                    var ps = await _CatalogDbContext.CustomerWallets.Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<CustomerWallet>();
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

        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomerByprofile(string profile,Ulid? idAgency,String? phone, int page = 1, int limit = 10)
        {
            ResponseBody<List<CustomerWallet>> rp = new ResponseBody<List<CustomerWallet>>();
            try
            {
                if(phone != null)
                {
                    phone = phone.Trim().Replace(" ","");
                }

                if (profile != null)
                {
                    profile = profile.Trim().Replace(" ", "");
                }

                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.CustomerWallets != null)
                {
                    var req = (idAgency != null && phone==null) ? _CatalogDbContext.CustomerWallets.Where(c => c.Profile == profile && c.FkIdAgency == idAgency):
                        (idAgency != null && phone != null)? _CatalogDbContext.CustomerWallets.Where(c => c.Profile == profile && c.FkIdAgency == idAgency && c.Phone.Contains(phone)) :
                        (idAgency == null && phone != null)? _CatalogDbContext.CustomerWallets.Where(c => c.Profile == profile && c.Phone.Contains(phone)) :
                        _CatalogDbContext.CustomerWallets.Where(c => c.Profile == profile) 
                        ;

                    int pageCount = (int)Math.Ceiling((decimal) req.Count() / limit);
                    var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<CustomerWallet>();
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
        public async Task<ResponseBody<List<CustomerWallet>>> getCustomerWalletDateBetweenAndAgent(DateTime begenDate, DateTime endDate, Ulid? idAgent,int page=1,int limit=10)
        {
            ResponseBody<List<CustomerWallet>> rp = new ResponseBody<List<CustomerWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.CustomerWallets != null)
                {
                    var req = (idAgent != null) ?
                         _CatalogDbContext.CustomerWallets
                         .Include(i => i.Accounting)
                       .Where(i => i.Profile == "AGENT")
                       .Where(i => i.IdCustomerWallet == idAgent)
                       .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate)
                        : 
                        _CatalogDbContext.CustomerWallets
                        .Include(i => i.Accounting)
                       .Where(i => i.Profile == "AGENT")
                       .Where(i => i.CreatedDate > begenDate && i.CreatedDate < endDate);
                    Console.WriteLine("Good");
                    Console.WriteLine(req);
                    int pageCount = (int)Math.Ceiling((decimal)req.Count() / limit);
                    var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();//string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<CustomerWallet>();
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
        public async Task<ResponseBody<List<CustomerWallet>>> findAllAgentsByAgency( Ulid? idAgency, int page = 1, int limit = 10)
        {
            ResponseBody<List<CustomerWallet>> rp = new ResponseBody<List<CustomerWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.CustomerWallets != null)
                {
                    var req =_CatalogDbContext.CustomerWallets.Where(c => c.Profile == "AGENT" && c.FkIdAgency == idAgency);
                    int pageCount = (int)Math.Ceiling((decimal)req.Count() / limit);
                    var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<CustomerWallet>();
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

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletById(Ulid id)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Include(c=>c.Accounting).Include(c=>c.Staff).Where(c=>c.IdCustomerWallet==id).FirstOrDefaultAsync();
                if (cus != null)
                {
                    rp.Body = cus;
                    return rp;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "User not found";
                    rp.Code = 450;
                    return rp;
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

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletCode(string code)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {

                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.Code == code).Include(c => c.Accounting).FirstOrDefaultAsync();
                if (cus != null)
                {
                    rp.Body = cus;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = cus.Profile+" not found";
                    rp.Code = 450;
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletContryidentityAndPhone(BodyPhoneShDto bd)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {


                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.phoneIdentity == bd.CountryIdentity && c.Phone == bd.Phone).Include(c=>c.Accounting).FirstOrDefaultAsync();
                if (cus != null)
                {
                    rp.Body = cus;
                }
                else
                {
                    rp.IsError = true;
                    rp.Code = 450;
                    rp.Msg = "Phone or Identity not correct";
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

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletPinContryidentityPhone(BodyLoginDto bd)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {


                CustomerWallet cus= await _CatalogDbContext.CustomerWallets.Where(c=>c.phoneIdentity==bd.CountryIdentity && c.Phone==bd.Phone).FirstOrDefaultAsync();
                if (cus != null)
                {
                    if (!BCrypt.Net.BCrypt.EnhancedVerify(cus.PinNumber,bd.pinNumber))
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or pin not correct";
                        rp.Code = 332;
                        return rp;
                    }

                    if (cus.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Code = 320;
                        rp.Msg = "Your account is blocked!";
                        return rp;
                    }
                    if (!cus.IsActive) {
                        rp.IsError = true;
                        rp.Code = 322;
                        rp.Msg = "Your account is not active!";
                        return rp;
                    }
                    rp.Body = cus;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Phone or pin not correct";
                    rp.Code = 332;
                    return rp;
                    
                }
               

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }
        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletPintempContryidentityPhone(BodyConfPinTempDto bd)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {


                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.phoneIdentity == bd.CountryIdentity && c.Phone == bd.Phone && c.PinTemp== bd.pinTempNumber).FirstOrDefaultAsync();
                if (cus != null)
                {
                    
                    cus.IsActive = true;
                    cus.UpdatedDate = DateTime.UtcNow;

                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body=cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Code = 332;
                    rp.Msg = "Phone or pin not correct";
                    return rp;
                   
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

        public async Task<ResponseBody<CustomerWallet>> updateCustomerInformations(CustomerUpdateInfosDto cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cu=await _CatalogDbContext.CustomerWallets.Where(c=>c.phoneIdentity==cus.PhoneIdentity && c.Phone==cus.Phone).FirstOrDefaultAsync();
                if (cu != null)
                {
                    cu.MiddleName=cus.MiddleName.Trim().Replace(" ","");
                    cu.FirstName=cus.FirstName.Trim().Replace(" ", "");
                    cu.LastName=cus.LastName.Trim().Replace(" ", "");
                    cu.Profile = cus.Profile.Trim().Replace(" ", "");
                    cu.PinNumber= BCrypt.Net.BCrypt.EnhancedHashPassword(cus.PinNumber.Trim().Replace(" ", ""));
                    cu.Address=cus.Address.Trim().Replace(" ", "");
                    cu.PhoneBrand=cus.PhoneBrand.Trim().Replace(" ", "");
                  if (cus.Profile =="AGENT")
                    {
                        cu.IsActive = false;
                    }else if(cus.Profile == "CUSTOMER")
                    {
                        cu.IsActive=true;
                    }
                    else
                    {
                        rp.IsError=true;
                        rp.Msg = "Profil does not exist";
                        rp.Body = null;
                        return rp;
                    }
                    
                    cu.UpdatedDate=DateTime.UtcNow;
                   
                    _CatalogDbContext.CustomerWallets.Update(cu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cu;
                }
                else
                {
                    rp.IsError = true;
                    rp.Code = 332;
                    rp.Msg = "Customer with phone "+cus.PhoneIdentity+ " "+cus.Phone +" not found";
                    return rp;
                   
                }

            }
            catch (Exception ex)
            {
                rp.Code = 400;
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<String>> updateCustomerPinNumberByStaff(UpdateCustomerByStaffDto cus)
        {
            ResponseBody<String> rp = new ResponseBody<String>();
            try
            {
                CustomerWallet cu = await _CatalogDbContext.CustomerWallets.Where(c => c.phoneIdentity == cus.PhoneIdentity && c.Phone == cus.PhoneNumber).FirstOrDefaultAsync();
                
                if (cu != null)
                {
                    Random rdn = new Random();
                    var a = rdn.Next(1000, 9999);
                    cu.PinNumber = BCrypt.Net.BCrypt.EnhancedHashPassword(a.ToString());
                   
                   //SMS send
                    cu.FkIdStaff = cus.IdStaff;
                    cu.UpdatedDate = DateTime.UtcNow;

                    _CatalogDbContext.CustomerWallets.Update(cu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = a.ToString();
                }
                else
                {
                    rp.IsError = true;
                    rp.Code = 332;
                    rp.Msg = "Customer with phone " + cus.PhoneIdentity + " " + cus.PhoneIdentity + " not found";
                    return rp;

                }

            }
            catch (Exception ex)
            {
                rp.Code = 400;
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<CustomerWallet>> updateCustomerInformationsWithoutPinNumber(CustomerUpdateDto cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cu = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == cus.Id ).FirstOrDefaultAsync();
                if (cu != null) 
                {
                    cu.MiddleName = cus.MiddleName;
                    cu.FirstName = cus.FirstName;
                    cu.LastName = cus.LastName;
                    cu.Profile = cus.Profile;
                    cu.Address = cus.Address;
                    cu.FkIdStaff= cus.FkIdStaff;
                    cu.UpdatedDate = DateTime.UtcNow;
                    if (cus.Profile != "AGENT" && cus.Profile!="CUSTOMER")
                   
                    {
                        rp.IsError = true;
                        rp.Msg = "Profil does not exist";
                        rp.Body = null;
                        return rp;
                    }

                    _CatalogDbContext.CustomerWallets.Update(cu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cu;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
                    rp.Code = 450;
                    return rp;

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

        public async Task<ResponseBody<CustomerWallet>> updateCustomerPin(CustomerUpadatePinDto cus)
        {
            
                ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
                try
                {
                    CustomerWallet cu = await _CatalogDbContext.CustomerWallets.Where(c => c.Phone == cus.Phone && c.phoneIdentity==cus.PhoneCountryIdentity).FirstOrDefaultAsync();
                    if (cu != null)
                    {
                    if (cu.PinNumber != cus.OldPinNumber.Trim().Replace(" ",""))
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or old pin not valide!";
                        rp.Body = null;
                        return rp;
                    }
                        
                        cu.PinNumber = BCrypt.Net.BCrypt.EnhancedHashPassword(cus.NewPinNumber.Trim().Replace(" ",""));

                        _CatalogDbContext.CustomerWallets.Update(cu);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = cu;
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or old pin not valide!";
                        rp.Code = 332;
                        return rp;
                    }


                }
                catch (Exception ex)
                {
                    rp.IsError = true;
                    rp.Code = 400;
                    rp.Msg = "error";
                }
                return rp;
            }
    }
}
