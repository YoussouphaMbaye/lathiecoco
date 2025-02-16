using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AgentOwnerRep
    {
        Task<ResponseBody<OwnerAgent>> login(LoginDto bd);
        Task<ResponseBody<BodyNbCountDto>> getStatistique();
        Task<ResponseBody<OwnerAgent>> findOwnerAgenctById(Ulid Id);
        Task<ResponseBody<OwnerAgent>> addOwnerAgent(BodyOwnerAgentDto oa);
        Task<ResponseBody<OwnerAgent>> updatePassword(ChangePasswordDto cp);
        Task<ResponseBody<OwnerAgent>> blockOrDeblockOwnerAgent(ActiveBlockDto dto);
        Task<ResponseBody<OwnerAgent>> activateOrDeactiveOwnerAgent(ActiveBlockDto dto);
        Task<ResponseBody<List<OwnerAgent>>> findAllOwnerAgents(int page = 1, int limit = 10);
        Task<ResponseBody<OwnerAgent>> updateOwnerAgent(BodyAgentOwnerUpdateDto oa, Ulid idOwnerAgent);
    }
}
