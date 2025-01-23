using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace  Lathiecoco.services
{
    public class FeeSendService : FeesSendRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public FeeSendService(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<FeeSend>> addFeesSend(FeeSend ac)
        {
            ResponseBody<FeeSend> rp = new ResponseBody<FeeSend>();
            try
            {
                ac.IdFeeSend= Ulid.NewUlid();

                await _CatalogDbContext.FeeSends.AddAsync(ac);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = ac;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
            }
            return rp;
        }

        public async Task<ResponseBody<List<FeeSend>>> addFeesSendWithPaymentMode(FeeSendBody fee, int[] PaymentModes)
        {
            ResponseBody<List<FeeSend>> rp = new ResponseBody<List<FeeSend>>();
            
               
            var transaction = _CatalogDbContext.Database.BeginTransaction();
            try
            {
                List<FeeSend> listFees= new List < FeeSend >();
                for (int j = 0; j < PaymentModes.Length; j++)
                {
                    FeeSend fs = new FeeSend();
                    
                    //fs.FkIdPaymentMode = j;
                   
                    fs.MinAmount = fee.MinAmount;
                    fs.MaxAmount = fee.MaxAmount;
                    fs.PercentAgFee = fee.PercentAgFee;
                    fs.PercentCsFee = fee.PercentCsFee;
                    fs.FixeAgFee = fee.FixeAgFee;
                    fs.FixeCsFee = fee.FixeCsFee;
                    listFees.Add(fs);
                    await _CatalogDbContext.FeeSends.AddAsync(fs);
                    await _CatalogDbContext.SaveChangesAsync();
                }
                transaction.Commit();
                rp.Body = listFees;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                rp.IsError = true;
                rp.Msg = ex.ToString();
            }
            return rp;
        }

        public async Task<ResponseBody<List<FeeSend>>> findAllFeeSend(int page = 1, int limit = 10)
        {
            ResponseBody<List<FeeSend>> rp = new ResponseBody<List<FeeSend>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.FeeSends != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.FeeSends.Count() / limit);
                    var ps = await _CatalogDbContext.FeeSends.Include(f=>f.PaymentMode).Skip(skip).Take(limit).OrderByDescending(c => c.CreatedDate).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<FeeSend>();
                    }
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


        

        public async Task<ResponseBody<FeeSend>> findWithPaymentMode(Ulid idPaymentMode)
        {
            ResponseBody<FeeSend> rp = new ResponseBody<FeeSend>();
            try
            {


                FeeSend fs=await _CatalogDbContext.FeeSends.Where(f=>f.FkIdPaymentMode== idPaymentMode).FirstOrDefaultAsync();
                if (fs != null)
                {
                    rp.Body = fs;
                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Fee not found";
                    rp.Code = 440;
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
    }
}
