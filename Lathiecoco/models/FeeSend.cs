using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class FeeSend : BaseEntity
    {
        [Key]
        public Ulid IdFeeSend { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public float FixeAgFee { get; set; } = 0;
        public float FixeCsFee { get; set; } = 0;
        public float PercentAgFee { get; set; } = 0;
        public float PercentCsFee { get; set; } = 0;
        public Ulid? FkIdPaymentMode { get; set; }
        public PaymentMode? PaymentMode { get; set; }
        public Ulid? FkIdStaff { get; set; }
        public OwnerAgent? Staff { get; set; }
        //public Ulid FkIdAgency { get; set; }
        //public Agency? Agency { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceWallet>? InvoiceWallet { get; set; }
        [JsonIgnore]
        public ICollection<BillerInvoice>? BillerInvoices { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWalletAgent>? InvoiceWalletAgents { get; set; }
       
    }
}
