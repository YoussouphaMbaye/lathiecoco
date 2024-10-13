using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class AccountingService : AccountingRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public AccountingService(CatalogDbContext catalogDbContext)
        {
            _CatalogDbContext = catalogDbContext;
        }
        public async Task<ResponseBody<Accounting>> addAccounting(Accounting ac)
        {
            ResponseBody<Accounting> rp = new ResponseBody<Accounting>();
            try
            {


                await _CatalogDbContext.Accountings.AddAsync(ac);
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

        public async Task<ResponseBody<List<Accounting>>> findAllAccounting(int page = 1, int limit = 10)
        {
            ResponseBody<List<Accounting>> rp = new ResponseBody<List<Accounting>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.Accountings != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.Accountings.Count() / limit);
                    var ps = await _CatalogDbContext.Accountings.Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<Accounting>();
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
