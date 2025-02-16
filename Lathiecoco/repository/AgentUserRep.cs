using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AgencyUserRep
    {
        Task<ResponseBody<AgencyUser>> login(LoginDto bd);
        Task<ResponseBody<AgencyUser>> updatePassword(ChangePasswordDto cp);
        Task<ResponseBody<AgencyUser>> findAgencyUsertById(Ulid Id);
        Task<ResponseBody<AgencyUser>> addAgencyUser(BodyAgencyUserDto oa);
        Task<ResponseBody<AgencyUser>> blockOrDeblockAgencyUser(ActiveBlockDto dto);
        Task<ResponseBody<AgencyUser>> activateOrDeactiveAgencyUser(ActiveBlockDto dto);
        Task<ResponseBody<List<AgencyUser>>> findAllAgentsUser(int page = 1, int limit = 10);
        Task<ResponseBody<AgencyUser>> updateAgencyUser(BodyAgencyUserUpdateDto oa, Ulid idAgencyUser);
        Task<ResponseBody<List<AgencyUser>>> findAgencyUsertByAgency(Ulid IdAgency, int page = 1, int limit = 10);
    }
}
