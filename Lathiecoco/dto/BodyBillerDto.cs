using Lathiecoco.models;

namespace Lathiecoco.dto
{
    public class BodyBillerDto
    {
        public Ulid IdCustomerWallet { get; set; }
        public string BillerReference { get; set; }
        public string ReloadBiller { get; set; }
        public string PayementType { get; set; }
        public double AmountToPaid { get; set; }
    }
}
