using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AccountingOpPrincipalRep
    {
        Task<ResponseBody<List<AccountingOpPrincipal>>> findAllAccountingOpPrincipals(int page = 1, int limit = 10);
        Task<ResponseBody<List<AccountingOpPrincipal>>> findAllAccountingOpPrincipalWithAccounting(Ulid idAccounting, int page = 1, int limit = 10);
    }
}
