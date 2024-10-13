using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface InvoiceAgentRep
    {
        Task<ResponseBody<InvoiceWalletAgent>> addInvoiceWallet(InvoiceWalletAgent ac);

        Task<ResponseBody<InvoiceWalletAgent>> withdraw(BodyInvoiceWalletCashier ac);
        Task<ResponseBody<InvoiceWalletAgent>> deposit(BodyInvoiceWalletCashier ac);
        Task<ResponseBody<List<InvoiceWalletAgent>>> findAllInvoiceWallet(int page = 1, int limit = 10);
        Task<ResponseBody<InvoiceWalletAgent>> findWalletCashierById(Ulid idInvoice);
    }
}
