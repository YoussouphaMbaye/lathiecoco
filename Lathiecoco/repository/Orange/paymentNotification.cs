using Lathiecoco.models;
using Lathiecoco.models.notifications;

namespace Lathiecoco.repository.Orange
{
    public interface paymentNotificationsRep
    {
        Task<ResponseBody<string>> orangeMoneyNotificationsHandler(Notifications om);
   

    }
}
