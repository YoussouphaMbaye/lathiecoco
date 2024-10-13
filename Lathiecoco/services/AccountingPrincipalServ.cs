using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class AccountingPrincipalServ : AccountingPrincipalRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public AccountingPrincipalServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<AccountingPrincipal>> addAgAccountingPrincipal(AccountingPrincipal acp)
        {
            ResponseBody<AccountingPrincipal> rp = new ResponseBody<AccountingPrincipal>();
            try
            {


                await _CatalogDbContext.AccountingPrincipals.AddAsync(acp);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = acp;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;

        }

        public async Task<ResponseBody<List<AccountingPrincipal>>> findAllAgAccountingPrincipal(int page = 1, int limit = 10)
        {
            ResponseBody<List<AccountingPrincipal>> rp = new ResponseBody<List<AccountingPrincipal>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AccountingPrincipals != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AccountingPrincipals.Count() / limit);
                    var ps = await _CatalogDbContext.AccountingPrincipals.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<AccountingPrincipal>();
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
