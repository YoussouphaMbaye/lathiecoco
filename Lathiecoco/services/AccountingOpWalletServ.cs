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

        public async Task<ResponseBody<List<AccountingOpWallet>>> searcheInvoiceWalletAgent(Ulid idAccounting,string? PaymentMethod, DateTime? beginDate, DateTime? endDate, int page=1, int limit=10)
        {
            ResponseBody<List<AccountingOpWallet>> rp = new ResponseBody<List<AccountingOpWallet>>();

            if (PaymentMethod != null)
            {
                PaymentMethod = PaymentMethod.Trim().Replace(" ", "");

            }


            try
            {
                DateTime myDateTime = DateTime.UtcNow;
                string dateNow = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if (endDate != null)
                {
                    dateNow = ((DateTime)endDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                string query = $"Select * from \"AccountingOpWallets\" where  \"FkIdAccounting\" = '{idAccounting}' ";

                query += $"and  \"CreatedDate\" <'{dateNow}' ";

               
                if (PaymentMethod != null)
                {
                    query += $"and  \"PaymentMode\" ='{PaymentMethod}' ";
                }
                if (beginDate != null)
                {
                    string beginDateTostring = ((DateTime)beginDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    query += $"and  \"CreatedDate\" >'{beginDateTostring}' ";
                }
                //query += $";";

                Console.WriteLine(dateNow);
                Console.WriteLine(query);
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.BillerInvoices != null)
                {
                    var totalItems =_CatalogDbContext.AccountingOpWallets.FromSqlRaw(query).Count();

                    int pageCount = (int)Math.Ceiling((decimal)totalItems / limit);
                    var ps =await _CatalogDbContext.AccountingOpWallets.FromSqlRaw(query).
                        Include(i => i.BillerInvoice.CustomerWallet).
                        Include(i => i.InvoiceStartupMaster).
                        Include(i => i.InvoiceWalletAgent.CustomerWallet).
                        Include(i => i.InvoiceWalletAgent).
                        OrderByDescending(c => c.UpdatedDate).Skip(skip).Take(limit).ToListAsync();
                    
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
                Console.WriteLine(ex.ToString());
                rp.IsError = true;
                rp.Code = 400;
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
