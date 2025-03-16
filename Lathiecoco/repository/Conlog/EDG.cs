using Lathiecoco.models.notifications;
using Lathiecoco.models;
using Lathiecoco.models.conlog;

namespace Lathiecoco.repository.Conlog
{
    public interface EDGrep 
    {
        Task<ResponseBody<_customer>> confirmCustomer(string numCompteur);
        Task<ResponseBody<AccountPaymentServicesEdg>> payCustomer(EdgPayment pay);
    }
}
