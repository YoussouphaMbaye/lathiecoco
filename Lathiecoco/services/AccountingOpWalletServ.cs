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
                    int totalCount = _CatalogDbContext.AccountingOpWallets.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
                    var ps = await _CatalogDbContext.AccountingOpWallets.Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalCount = totalCount;
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
                rp.Code = 400;

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
                    var req = _CatalogDbContext.AccountingOpWallets.Where(a => a.FkIdAccounting == idAccounting);
                    var totalCount = req.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
                    var ps = await req.
                        Include(i=>i.BillerInvoice.CustomerWallet).
                        Include(i => i.InvoiceStartupMaster).
                        Include(i => i.InvoiceWalletAgent.CustomerWallet).
                        Include(i => i.InvoiceWalletAgent).Where(a=>a.FkIdAccounting==idAccounting).OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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
                rp.Code = 400;

            }
            return rp;
        }
    }
}
