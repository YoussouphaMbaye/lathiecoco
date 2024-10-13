using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface InvoiceWalletRep
    {
        Task<ResponseBody<InvoiceWallet>> addInvoiceWallet(InvoiceWallet ac);
        Task<ResponseBody<InvoiceWallet>> invoiceWalletWithId(Ulid id);

        Task<ResponseBody<InvoiceWallet>> insertInvoiceWallet(BodyPostInvoiceWallet ac);
        Task<ResponseBody<List<InvoiceWallet>>> findAllInvoiceWallet(int page = 1, int limit = 10);
    }
}
