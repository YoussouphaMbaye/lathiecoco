using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface PaymentModeRep
    {
        Task<ResponseBody<PaymentMode>> findByPaymentMode(string pm);
        Task<ResponseBody<PaymentMode>> addPaymentMode(PaymentMode pm);
        Task<ResponseBody<PaymentMode>> updatePaymentMode(PaymentModeDto pm);
        Task<ResponseBody<List<PaymentMode>>> findAllPaymentMode(int page = 1, int limit = 10);
        Task<ResponseBody<PaymentMode>> activateOrDeactivatePaymentMode(ActiveDeactivePaymentModeDto acPm);
    }
}
