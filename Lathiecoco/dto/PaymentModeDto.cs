namespace Lathiecoco.dto
{
    public class PaymentModeDto
    {
        public string Name { get; set; }
        public Ulid FkIdStaff { get; set; }
        public Ulid IdPaymentMode { get; set; }
        public string Description { get; set; }
    }
}
