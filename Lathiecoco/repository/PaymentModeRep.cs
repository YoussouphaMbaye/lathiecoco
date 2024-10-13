using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface PaymentModeRep
    {
        Task<ResponseBody<PaymentMode>> addPaymentMode(PaymentMode pm);
        Task<ResponseBody<PaymentMode>> findByPaymentMode(string pm);
        Task<ResponseBody<List<PaymentMode>>> findAllPaymentMode(int page = 1, int limit = 10);
    }
}
