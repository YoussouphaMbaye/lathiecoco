
namespace Lathiecoco.models.mtn
{

    public class Payer
    {
        public string partyIdType { get; set; }
        public string partyId { get; set; }
    }
    public class requestToPay
    {
            public string? amount { get; set; }
            public string? currency { get; set; }
            public string? externalId { get; set; }
            public Payer ? payer { get; set; }
            public string? payerMessage { get; set; }
            public string? payeeNote { get; set; }
            public string? status { get; set; }
            public string? reason { get; set; }
    }

    public class mtnPaymentRequest
    {
        public string ? amount { get; set; }
        public string? partnerId { get; set; }
        public string? phoneNumber { get; set; }
    }

    public class payee
    {
        public string partyIdType { get; set; }
        public string partyId { get; set; }
    }
    public class mtnNotifications
    {
        public string? amount { get; set; }
        public string? currency { get; set; }
        public string? externalId { get; set; }
        public payee? payee { get; set; }
        public string? payerMessage { get; set; }
        public string? payeeNote { get; set; }
        public string? status { get; set; }
        public string? reason { get; set; }
    }
}
