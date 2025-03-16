
using Lathiecoco.models;
using Lathiecoco.models.orange;

namespace Lathiecoco.repository.Orange
{
    public interface OrangeRep
    {
        Task<ResponseBody<string>> transactionsProcess(TransactionsOrange om);
    }
}
