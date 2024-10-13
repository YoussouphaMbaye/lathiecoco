using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AccountingRep
    {
        Task<ResponseBody<Accounting>> addAccounting(Accounting ac);
        Task<ResponseBody<List<Accounting>>> findAllAccounting(int page = 1, int limit = 10);
    }
}
