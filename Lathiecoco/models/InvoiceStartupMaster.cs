using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class InvoiceStartupMaster:BaseEntity
    {
        [Key]
        public Ulid IdInvoiceStartupMaster { get; set; }
        public string InvoiceCode { get; set; }
        public string? InvoiceCode2 { get; set; }
        public string PaymentMode { get; set; }
        public bool IsMaster { get; set; }
        public string InvoiceStatus { get; set; }
        public string? ProofLink { get; set; }
        public double AmountToSend { get; set; }
        public double AmountToPaid { get; set; }
        //public double AmountToReceived { get; set; }
        public Ulid FkIdPaymentMode { get; set; }
        public PaymentMode? PaymentModeObj { get; set; }
        public DateTime? ValidateAt { get; set; }
        public Ulid? FkIdStaff { get; set; }
        public OwnerAgent? Staff { get; set; }
        public Ulid? FkIdAgent { get; set; }
        public CustomerWallet? Agent { get; set; }
        public Ulid? FkIdAgencyUser { get; set; }
        public AgencyUser? AgencyUser { get; set; }
        [JsonIgnore]
        public ICollection<AccountingOpWallet>? AccountingOpWallet { get; set; }
        [JsonIgnore]
        public ICollection<AccountingOpPrincipal>? AccountingOpPrincipals { get; set; }
    }
}
