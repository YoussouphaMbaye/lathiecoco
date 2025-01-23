using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogsController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private UserLogRep _userLogServ;
        public UserLogsController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            UserLogRep userLogServ)
        {
            this.catalogDbContext = catalogDbContext;

            this._environnement = environnement;

            _configuration = configuration;

            _userLogServ = userLogServ;
        }
        [HttpGet("/userLogs/findWithUser")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<UserLog>>> findWithUser(Ulid? fkIdStaff, DateTime beginDate, DateTime endDate, int page = 1, int limit = 10)
        {

            return await _userLogServ.findUserLogByStaff(fkIdStaff, beginDate, endDate, page, limit);

        }
        [HttpPost("/userLogs/addUserLogs")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<UserLog>> addUserLogs(AddUserLogDto dto)
        {

            return await _userLogServ.addUserLog(dto);

        }
    }
}
