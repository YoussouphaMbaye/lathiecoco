﻿using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AgentOwnerRep
    {
        Task<ResponseBody<OwnerAgent>> addOwnerAgent(BodyOwnerAgentDto oa);
        Task<ResponseBody<OwnerAgent>> login(LoginDto bd);
        Task<ResponseBody<OwnerAgent>> updateOwnerAgent(BodyAgentOwnerUpdateDto oa, Ulid idOwnerAgent);
        Task<ResponseBody<BodyNbCountDto>> getStatistique();
        Task<ResponseBody<OwnerAgent>> findOwnerAgenctById(Ulid Id);
        Task<ResponseBody<List<OwnerAgent>>> findAllOwnerAgents(int page = 1, int limit = 10);
    }
}
