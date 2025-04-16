using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;

namespace  Lathiecoco.services
{
    public class PaymentModeServ : PaymentModeRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public PaymentModeServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<PaymentMode>> addPaymentMode(PaymentMode pm)
        {
            ResponseBody<PaymentMode> rp = new ResponseBody<PaymentMode>();
            try
            {
                pm.IdPaymentMode= Ulid.NewUlid();

                await _CatalogDbContext.PaymentModes.AddAsync(pm);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = pm;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }
        public async Task<ResponseBody<PaymentMode>> findByPaymentMode(string pm)
        {
            ResponseBody<PaymentMode> rp = new ResponseBody<PaymentMode>();
            try
            {


                PaymentMode p=await _CatalogDbContext.PaymentModes.Where(p=>p.Name==pm).FirstOrDefaultAsync();
                if(p!=null)
                {
                    rp.Body = p;
                }else
                {
                    rp.Body = null;
                    rp.IsError=true;
                    rp.Msg="PaymentMode " + pm + " not found";
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

        public async Task<ResponseBody<List<PaymentMode>>> findAllPaymentMode(int page = 1, int limit = 10)
        {
            ResponseBody<List<PaymentMode>> rp = new ResponseBody<List<PaymentMode>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.PaymentModes != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.PaymentModes.Count() / limit);
                    var ps = await _CatalogDbContext.PaymentModes.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<PaymentMode>();
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
