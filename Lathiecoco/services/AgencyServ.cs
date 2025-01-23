using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace Lathiecoco.services
{
    public class AgencyServ : AgencyRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public AgencyServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<Agency>> addAgency(AgencyDto ag)
        {
            ResponseBody<Agency> rp = new ResponseBody<Agency>();
            var transaction = _CatalogDbContext.Database.BeginTransaction();
            try
            {
                Agency ag1=await _CatalogDbContext.Agencies.Where(a=>a.phone==ag.phone).FirstOrDefaultAsync();
                
                if (ag1!=null) {
                    rp.IsError = true;
                    rp.Msg = "Agency " + ag.phone + " already exist";
                    rp.Code = 400;
                    return rp;
                }
                Accounting ac = new Accounting();
                ac.IdAccounting = Ulid.NewUlid();
                ac.Currency = "GNF";
                ac.Balance = 0;
                ac.CreatedDate = DateTime.Now;
                ac.UpdatedDate = DateTime.Now;

                await _CatalogDbContext.Accountings.AddAsync(ac);
                await _CatalogDbContext.SaveChangesAsync();

                Agency agency = new Agency();
                agency.IdAgency = Ulid.NewUlid();
                agency.isActive = true;
                agency.FkIdAccounting = ac.IdAccounting;
                agency.name=ag.name.ToUpper();
                agency.email=ag.email;
                agency.phone=ag.phone;
                string newcode =GlobalFunction.ConvertToUnixTimestamp(DateTime.Now);
                agency.code= ag.name.ToUpper().Substring(0,3)+ newcode.Substring(newcode.Length-4);
                //agency.login=ag.login;
                agency.CreatedDate=DateTime.Now;
                agency.UpdatedDate=DateTime.Now;
                agency. FkIdStaff=ag.fkIdStaff;

                await _CatalogDbContext.Agencies.AddAsync(agency);
                await _CatalogDbContext.SaveChangesAsync();

                transaction.Commit();
                rp.Body = agency;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }
        public async Task<ResponseBody<Agency>> updateAgency(AgencyPutDto ag,Ulid idAgency)
        {
            ResponseBody<Agency> rp = new ResponseBody<Agency>();
            
            try
            {
                Agency agency = await _CatalogDbContext.Agencies.Where(a => a.IdAgency== idAgency).FirstOrDefaultAsync();

                if (agency != null)
                {
                    agency.email = ag.email;
                    agency.phone = ag.phone.Trim().Replace(" ","");
                    agency.name = ag.name.ToUpper();
                    agency.UpdatedDate = DateTime.Now;
                    _CatalogDbContext.Agencies.Update(agency);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = agency;
                }
                else
                {
                    
                    rp.IsError = true;
                    rp.Msg = "Agency not found";
                    rp.Code = 403;
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

        public async Task<ResponseBody<Agency>> definePercentagePurchase(DefinePercentagePurchaseAgentDto dto)
        {
            ResponseBody<Agency> rp = new ResponseBody<Agency>();
            try
            {
                Agency ag = await _CatalogDbContext.Agencies.Where(c => c.IdAgency == dto.IdAgency).FirstOrDefaultAsync();
                if (ag != null)
                {

                    ag.PercentagePurchase = dto.Percentage;
                    ag.UpdatedDate = DateTime.Now;
                    ag.FkIdStaff=dto.IdStaff;

                    _CatalogDbContext.Agencies.Update(ag);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = ag;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Agency not found";
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
        public async Task<ResponseBody<Agency>> getAgencyById(Ulid id)
        {
            ResponseBody<Agency> rp = new ResponseBody<Agency>();
            var transaction = _CatalogDbContext.Database.BeginTransaction();
            try
            {
                Agency ag = await _CatalogDbContext.Agencies.Include(x => x.Staff).Include(x => x.Accounting).Where(a => a.IdAgency == id).FirstOrDefaultAsync();
                if (ag != null)
                {
                    rp.Body=ag;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Agency not found";
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
         public async Task<ResponseBody<List<Agency>>> agencySearch(string? email, string? code, string? phone, int page = 1, int limit = 10)
        {
            string sql = "select * from Agencies where isActive=1";

            if (code != null)
            {
                sql += " and code LIKE '%" + code + "%'";
            }
            if (email != null)
            {
                sql += " and email LIKE '%" + email + "%'";
            }
            if (phone != null)
            {
                sql += " and phone LIKE '%" + phone + "%'";
            }
            //sql += ";";
            Console.WriteLine(sql);
            ResponseBody<List<Agency>> rp = new ResponseBody<List<Agency>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.Agencies != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.Agencies.FromSqlRaw(sql).Count() / limit);

                    var ps = await _CatalogDbContext.Agencies.FromSqlRaw(sql).Include(e => e.Staff).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();

                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                         rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<Agency>();
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
            
        public async Task<ResponseBody<List<Agency>>> findAgencies(int page = 1, int limit = 10)
        {
            ResponseBody<List<Agency>> rp = new ResponseBody<List<Agency>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.Agencies != null)
                {
                    rp.TotalCount = _CatalogDbContext.Agencies.Count();
                    int pageCount = (int)Math.Ceiling((decimal) rp.TotalCount/ limit);
                    var ps = await _CatalogDbContext.Agencies.Include(c => c.Staff).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<Agency>();
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

    }
}
