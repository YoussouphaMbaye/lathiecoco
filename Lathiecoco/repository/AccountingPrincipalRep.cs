using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface AccountingPrincipalRep
    {
        Task<ResponseBody<AccountingPrincipal>> addAgAccountingPrincipal(AccountingPrincipal acp);
        Task<ResponseBody<List<AccountingPrincipal>>> findAllAgAccountingPrincipal(int page = 1, int limit = 10);
    }
}
