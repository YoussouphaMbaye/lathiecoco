using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface BilllerInvoiceRep
    {
        Task<ResponseBody<BillerInvoice>> insertBillerInvoice(BodyBillerDto biller);
        Task<ResponseBody<List<BillerInvoice>>> findAllBillerInvoice(int page = 1, int limit = 10);
        Task<ResponseBody<BillerInvoice>> findBillerInvoiceById(Ulid id);
    }
}
