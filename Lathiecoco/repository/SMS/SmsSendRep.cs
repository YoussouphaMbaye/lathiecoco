using Lathiecoco.models;

namespace Lathiecoco.repository.SMS
{
    public interface SmsSendRep
    {
        public Task<ResponseBody<string>> sendSms(string phoneNumber, string message);
    }
}
