using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseBody<CustomerWallet>> activateWallet(Ulid id)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == id).FirstOrDefaultAsync();
                if (cus != null)
                {
                    
                     cus.IsActive = !cus.IsActive;
                     _CatalogDbContext.CustomerWallets.Update(cus);
                     await _CatalogDbContext.SaveChangesAsync();
                     rp.Body = cus;
                     return rp;
                    
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
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
        public async Task<ResponseBody<CustomerWallet>> blokeOrDeblokeWallet(Ulid id)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Where(c => c.IdCustomerWallet == id).FirstOrDefaultAsync();
                if (cus != null)
                {

                    cus.IsBlocked = !cus.IsBlocked;
                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
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
                
               
               
                double ConvertToUnixTimestamp(DateTime date)
                {
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan diff = date.ToUniversalTime() - origin;
                    return Math.Floor(diff.TotalSeconds);
                }
               
                string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
                c.Code = "N" + dayToday;

                c.CreatedDate=DateTime.Now;
                c.UpdatedDate=DateTime.Now;
              
                
                Accounting ac = new Accounting();
                //ac.Currency = cp.Country.CurrencyName;
                ac.IdAccounting= Ulid.NewUlid();
                ac.Balance = 0;
                ac.CreatedDate=DateTime.Now;
                ac.UpdatedDate=DateTime.Now;
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
                    rp.Msg= ex.ToString();
                    rp.IsError = true;
                    rp.Body = null;
                }
               

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
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
                //c.PinTemp = a.ToString();
                c.PinTemp = "1234";
                string dayToday = GlobalFunction.ConvertToUnixTimestamp(DateTime.Now);
                c.Code = "C" + dayToday;
                c.CreatedDate = DateTime.Now;
                c.UpdatedDate = DateTime.Now;

                Accounting ac = new Accounting();
                ac.IdAccounting = Ulid.NewUlid();
                ac.Currency = "GNF";
                ac.Balance = 0;
                ac.CreatedDate = DateTime.Now;
                ac.UpdatedDate = DateTime.Now;
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
                    rp.Msg = ex.ToString();
                    rp.IsError = true;
                    rp.Body = null;
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
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

            }
            return rp;
        }

        public async Task<ResponseBody<List<CustomerWallet>>> findAllCustomerByprofile(string profile, int page = 1, int limit = 10)
        {
            ResponseBody<List<CustomerWallet>> rp = new ResponseBody<List<CustomerWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.CustomerWallets != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.CustomerWallets.Where(c=>c.Profile==profile).Count() / limit);
                    var ps = await _CatalogDbContext.CustomerWallets.Where(c => c.Profile == profile).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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

            }
            return rp;
        }

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletById(Ulid id)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cus = await _CatalogDbContext.CustomerWallets.Include(c=>c.Accounting).Where(c=>c.IdCustomerWallet==id).FirstOrDefaultAsync();
                if (cus != null)
                {
                    rp.Body = cus;
                    return rp;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "User not found";
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
                    rp.Msg = "Phone or Identity not correct";
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<CustomerWallet>> findCustomerWalletPinContryidentityPhone(BodyLoginDto bd)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {


                CustomerWallet cus= await _CatalogDbContext.CustomerWallets.Where(c=>c.phoneIdentity==bd.CountryIdentity && c.Phone==bd.Phone && c.PinNumber==bd.pinNumber).FirstOrDefaultAsync();
                if (cus != null)
                {
                    if (cus.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account is blocked!";
                        return rp;
                    }
                    if (!cus.IsActive) {
                        rp.IsError = true;
                        rp.Msg = "Your account is not active!";
                        return rp;
                    }
                    rp.Body = cus;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Phone or pin not correct";
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
                    cus.UpdatedDate = DateTime.Now;

                    _CatalogDbContext.CustomerWallets.Update(cus);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body=cus;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Phone or pin not correct";
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

        public async Task<ResponseBody<CustomerWallet>> updateCustomerInformations(CustomerUpdateInfosDto cus)
        {
            ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
            try
            {
                CustomerWallet cu=await _CatalogDbContext.CustomerWallets.Where(c=>c.phoneIdentity==cus.PhoneIdentity && c.Phone==cus.Phone).FirstOrDefaultAsync();
                if (cu != null)
                {
                    cu.MiddleName=cus.MiddleName;
                    cu.FirstName=cus.FirstName;
                    cu.LastName=cus.LastName;
                    cu.Profile = cus.Profile;
                    cu.PinNumber=cus.PinNumber;
                    cu.Address=cus.Address;
                    cu.PhoneBrand=cus.PhoneBrand;
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
                    
                    cu.UpdatedDate=DateTime.Now;
                   
                    _CatalogDbContext.CustomerWallets.Update(cu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cu;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer with phone "+cus.PhoneIdentity+ " "+cus.Phone +" not found";
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
                    if (cus.Profile != "AGENT" && cus.Profile!="CUSTOMER")
                   
                    {
                        rp.IsError = true;
                        rp.Msg = "Profil does not exist";
                        rp.Body = null;
                        return rp;
                    }

                    cu.UpdatedDate = DateTime.Now;

                    _CatalogDbContext.CustomerWallets.Update(cu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = cu;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Customer not found";
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

        public async Task<ResponseBody<CustomerWallet>> updateCustomerPin(CustomerUpadatePinDto cus)
        {
            
                ResponseBody<CustomerWallet> rp = new ResponseBody<CustomerWallet>();
                try
                {
                    CustomerWallet cu = await _CatalogDbContext.CustomerWallets.Where(c => c.Phone == cus.Phone && c.phoneIdentity==cus.PhoneCountryIdentity).FirstOrDefaultAsync();
                    if (cu != null)
                    {
                    if (cu.PinNumber != cus.OldPinNumber)
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or old pin not valide!";
                        rp.Body = null;
                        return rp;
                    }
                        
                        cu.PinNumber = cus.NewPinNumber;

                        _CatalogDbContext.CustomerWallets.Update(cu);
                        await _CatalogDbContext.SaveChangesAsync();
                        rp.Body = cu;
                    }
                    else
                    {
                        rp.IsError = true;
                        rp.Msg = "Phone or old pin not valide!";
                        return rp;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    rp.IsError = true;
                    rp.Msg = "error";
                }
                return rp;
            }
    }
}
