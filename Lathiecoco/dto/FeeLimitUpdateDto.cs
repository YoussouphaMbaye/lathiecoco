namespace Lathiecoco.dto
{
    public class FeeLimitUpdateDto
    {
        public Ulid feeId { get; set; }
        public Ulid fkIdStaff { get; set; }
        public double maxAmount { get; set; }
        public double minAmount {  get; set; }
        

    }
}
