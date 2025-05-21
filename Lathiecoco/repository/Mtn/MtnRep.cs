using Lathiecoco.models.orange;
using Lathiecoco.models;
using Lathiecoco.models.mtn;

namespace Lathiecoco.repository.Mtn
{
    public interface MtnRep
    {
        Task<ResponseBody<string>> MtnTransactionProcess(mtnPaymentRequest mtn);
        Task<ResponseBody<string>> mtnMoneyNotificationsHandler(requestToPay rp);

    }
}
