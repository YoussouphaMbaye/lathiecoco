using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface BilllerInvoiceRep
    {
        Task<ResponseBody<BillerInvoice>> findBillerInvoiceById(Ulid id);
        Task<ResponseBody<BillerInvoice>> updateBillerInvoiceToPaidByIdRef(Guid idRef);
        Task<ResponseBody<BillerInvoice>> insertBillerInvoice(BodyBillerDto biller);
        Task<ResponseBody<List<BillerInvoice>>> findAllBillerInvoice(int page = 1, int limit = 10);
        Task<ResponseBody<List<BillerAmountByAgentDto>>> billerByAgentSumBiller(DateTime begenDate, DateTime endDate, Ulid? idAgent, Ulid? fkIdAgency);
        Task<ResponseBody<List<BillerInvoice>>> searcheBillerInvoice(string? idPaymentMode, string? code, DateTime? beginDate, DateTime? endDate, String? phone, String? billerReference, string? invoiceStatus, int page = 1, int limit = 10);

    }
}
