using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class BillerInvoice:BaseEntity
    {
        [Key]
        public Ulid IdBillerInvoice { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceStatus { get; set; }
        public string BillerReference { get; set; }
        public string ReloadBiller { get; set; }
        public double AmountToPaid { get; set; }
        public double FeesAmount { get; set; }
        public double? NumberOfKw { get; set; }
        public string PaymentMode { get; set; }
        public Ulid? FkIdCustomerWallet { get; set; }
        public CustomerWallet? CustomerWallet { get; set; }
        public Ulid? FkIdPartener { get; set; }
        public Partener? Partener { get; set; }
        public Ulid? FkIdPaymentMode { get; set; }
        public PaymentMode? PaymentModeObj { get; set; }
        public Ulid? FkIdFeeSend { get; set; }
        public Guid? IdReference { get; set; }
        public string? BillerUserName { get; set; }
        public FeeSend? FeeSend { get; set; }
        [JsonIgnore]
        public ICollection<AccountingOpWallet>? AccountingOp { get; set; }
    }
}
