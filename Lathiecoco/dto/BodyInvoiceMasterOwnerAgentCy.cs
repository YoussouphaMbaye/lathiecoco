namespace Lathiecoco.dto
{
    public class BodyInvoiceMasterOwnerAgentCy
    {
        public Ulid FkIdMasterAgency {  get; set; }
        public Ulid FkIdOwnerAgent { get; set; }
        public string CodeSender { get; set; }
        public double AmountToSend { get; set; }
        
    }
}
