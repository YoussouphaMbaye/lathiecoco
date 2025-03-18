using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "ADMIN,SUPADMIN")]
        [HttpGet("/userLogs/findWithUser")]
        public async Task<ResponseBody<List<UserLog>>> findWithUser(String? email, DateTime beginDate, DateTime endDate, int page = 1, int limit = 10)
        {

            return await _userLogServ.findUserLogByStaff(email, beginDate, endDate, page, limit);

        }

        [Authorize(Roles = "ADMIN,SUPADMIN")]
        [HttpPost("/userLogs/addUserLogs")]
        public async Task<ResponseBody<UserLog>> addUserLogs(AddUserLogDto dto)
        {

            return await _userLogServ.addUserLog(dto);

        }
    }
}
