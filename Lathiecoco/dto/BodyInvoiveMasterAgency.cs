namespace Lathiecoco.dto
{
    public class BodyInvoiveMasterAgency
    {
        public Ulid FkIdMaster { get; set; }
        public Ulid FkIdCashier { get; set; }
        public double AmountToSend { get; set; }
    }
}
