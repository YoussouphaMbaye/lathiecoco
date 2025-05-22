
using Lathiecoco.models;
using Lathiecoco.models.notifications;
using Lathiecoco.models.orange;

namespace Lathiecoco.repository.Orange
{
    public interface OrangeRep
    {
        Task<ResponseBody<Notifications>> transactionsProcess(OrangePaymentMethod om);
    }
}
