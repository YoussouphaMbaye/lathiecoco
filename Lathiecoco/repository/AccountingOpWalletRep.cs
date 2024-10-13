using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AccountingOpWalletRep
    {
        //Task<ResponseBody<Accounting>> addAccounting(Accounting ac);
        Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWallet(int page = 1, int limit = 10);
        Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWalletWithAccounting(Ulid idAccounting, int page = 1, int limit = 10);
    }
}
