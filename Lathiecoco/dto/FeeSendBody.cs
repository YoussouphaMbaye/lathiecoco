namespace Lathiecoco.dto
{
    public class FeeSendBody
    {
        public double MinAmount { get; set; } = 0;
        public double MaxAmount { get; set; }= 0;
        public float FixeAgFee { get; set; } = 0;
        public float FixeCsFee { get; set; } = 0;
        public float PercentAgFee { get; set; } = 0;
        public float PercentCsFee { get; set; } = 0;
        public Ulid FkIdPaymentMode { get; set; }
        public Ulid FkIdAgency { get; set; }
        public Ulid FkIdCorridor { get; set; }
     
    }
}
