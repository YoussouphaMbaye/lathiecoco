using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lathiecoco.services
{
    public class UserLogServ : UserLogRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        public UserLogServ(CatalogDbContext CatalogDbContext)
        {
            _CatalogDbContext = CatalogDbContext;
        }
        public async Task<ResponseBody<UserLog>> addUserLog(AddUserLogDto dto)
        {
            ResponseBody<UserLog> rp = new ResponseBody<UserLog>();
            try
            {

                UserLog userLog = new UserLog();
                userLog.IdUserLog = Ulid.NewUlid();
                userLog.FkIdStaff=dto.FkIdStaff;
                userLog.CreatedDate=DateTime.UtcNow;
                userLog.UpdatedDate = DateTime.UtcNow;
                userLog.IPaddress=dto.IPaddress;
                userLog.UserAction=dto.UserAction;
                userLog.Code=dto.Code;

                await _CatalogDbContext.UserLogs.AddAsync(userLog);
                await _CatalogDbContext.SaveChangesAsync();

                rp.Body = userLog;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Code = 400;
                Console.WriteLine(ex.ToString());
            }
            return rp;
        }

        public async Task<ResponseBody<List<UserLog>>> findAllUserLog(int page = 1, int limit = 10)
        {
            ResponseBody<List<UserLog>> rp = new ResponseBody<List<UserLog>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.UserLogs != null)
                {   
                    var totalCount = _CatalogDbContext.UserLogs.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
                    var ps = await _CatalogDbContext.UserLogs.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
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
                        rp.Body = new List<UserLog>();
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
    

        public async Task<ResponseBody<List<UserLog>>> findUserLogByStaff(Ulid? fkIdStaff,DateTime beginDate, DateTime endDate, int page = 1, int limit = 10)
        {
            ResponseBody<List<UserLog>> rp = new ResponseBody<List<UserLog>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.UserLogs != null)
                {
                    var req = fkIdStaff!=null? _CatalogDbContext.UserLogs
                        .Where(x => x.FkIdStaff == fkIdStaff)
                        .Include(x => x.Staff)
                        .Where(x => x.CreatedDate > beginDate && x.CreatedDate < endDate)
                        : _CatalogDbContext.UserLogs
                        .Include(x => x.Staff)
                        .Where(x => x.CreatedDate > beginDate && x.CreatedDate < endDate);
                    var totalCount =req.Count();
                    int pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
                    var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalCount=totalCount;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<UserLog>();
                        
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
