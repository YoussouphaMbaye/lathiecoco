using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class AccountingOpPrincipalServ : AccountingOpPrincipalRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public AccountingOpPrincipalServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<List<AccountingOpPrincipal>>> findAllAccountingOpPrincipals(int page = 1, int limit = 10)
        {
            ResponseBody<List<AccountingOpPrincipal>> rp = new ResponseBody<List<AccountingOpPrincipal>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AccountingOpPrincipals != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AccountingOpPrincipals.Count() / limit);
                    var ps = await _CatalogDbContext.AccountingOpPrincipals.Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<AccountingOpPrincipal>();
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

        public async Task<ResponseBody<List<AccountingOpPrincipal>>> findAllAccountingOpPrincipalWithAccounting(Ulid idAccounting, int page = 1, int limit = 10)
        {

            ResponseBody<List<AccountingOpPrincipal>> rp = new ResponseBody<List<AccountingOpPrincipal>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AccountingOpPrincipals != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AccountingOpPrincipals.Count() / limit);
                    var ps = await _CatalogDbContext.AccountingOpPrincipals.Where(a => a.FkIdAccounting == idAccounting).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<AccountingOpPrincipal>();
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
