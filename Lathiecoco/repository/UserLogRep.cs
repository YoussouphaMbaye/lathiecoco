using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface UserLogRep
    {
        Task<ResponseBody<UserLog>> addUserLog(AddUserLogDto dto);
        Task<ResponseBody<List<UserLog>>> findAllUserLog(int page = 1, int limit = 10);
        Task<ResponseBody<List<UserLog>>> findUserLogByStaff(String? email, DateTime beginDate, DateTime endDate, int page = 1, int limit = 10);
    }
}
