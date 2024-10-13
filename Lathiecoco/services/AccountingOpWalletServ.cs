using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class AccountingOpWalletServ : AccountingOpWalletRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public AccountingOpWalletServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWallet(int page = 1, int limit = 10)
        {
            ResponseBody<List<AccountingOpWallet>> rp = new ResponseBody<List<AccountingOpWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AccountingOpWallets != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AccountingOpWallets.Count() / limit);
                    var ps = await _CatalogDbContext.AccountingOpWallets.Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<AccountingOpWallet>();
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

        public async Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWalletWithAccounting(Ulid idAccounting,int page = 1, int limit = 10)
        {
            ResponseBody<List<AccountingOpWallet>> rp = new ResponseBody<List<AccountingOpWallet>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AccountingOpWallets != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AccountingOpWallets.Count() / limit);
                    var ps = await _CatalogDbContext.AccountingOpWallets.Include(i=>i.BillerInvoice).Include(i => i.InvoiceStartupMaster).Include(i => i.InvoiceWalletAgent).Where(a=>a.FkIdAccounting==idAccounting).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;
                        return rp;

                    }
                    else
                    {
                        rp.Body = new List<AccountingOpWallet>();
                        return rp;
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
