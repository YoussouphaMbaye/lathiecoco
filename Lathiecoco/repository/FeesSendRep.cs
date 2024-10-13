using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface FeesSendRep
    {
        Task<ResponseBody<FeeSend>> addFeesSend(FeeSend ac);
        Task<ResponseBody<List<FeeSend>>> addFeesSendWithPaymentMode(FeeSendBody ac, int[] PaymentModes);
        Task<ResponseBody<FeeSend>> findWithPaymentMode(Ulid idPaymentMode);
        //Task<ResponseBody<List<FeeSend>>> findWithCorridor(Ulid FkIdCorridor, int page = 1, int limit = 10);
        Task<ResponseBody<List<FeeSend>>> findAllFeeSend(int page = 1, int limit = 10);
    }
}
