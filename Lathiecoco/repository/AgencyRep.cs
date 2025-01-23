using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface AgencyRep
    {
        Task<ResponseBody<Agency>> addAgency(AgencyDto s);
        Task<ResponseBody<Agency>> getAgencyById(Ulid id);
        Task<ResponseBody<Agency>> updateAgency(AgencyPutDto ag, Ulid idAgency);
        Task<ResponseBody<List<Agency>>> findAgencies(int page = 1, int limit = 10);
        Task<ResponseBody<Agency>> definePercentagePurchase(DefinePercentagePurchaseAgentDto dto);
        Task<ResponseBody<List<Agency>>> agencySearch(string? email, string? code, string? phone, int page = 1, int limit = 10);
    }
}
