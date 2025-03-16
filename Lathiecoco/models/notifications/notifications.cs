namespace Lathiecoco.models.notifications
{
    public class Notifications
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public TransactionData? transactionData { get; set; }
    }

    public class TransactionData
    {
        public string? type { get; set; }
        public string? peerId { get; set; }
        public string? peerIdType { get; set; }
        public double amount { get; set; }
        public string? currency { get; set; }
        public string? posId { get; set; }
        public string? transactionId { get; set; }
    }

}
