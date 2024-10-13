using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class InvoiceWalletAgent:BaseEntity
    {
        [Key]
        public Ulid IdInvoiceWalletCashier { get; set; }
        public string InvoiceCode { get; set; }
        public string? InvoiceCode2 { get; set; }
        public string PaymentMode { get; set; }
        public Ulid FkIdAgent { get; set; }
        public Ulid FkIdCustomerWallet { get; set; }
        public string InvoiceStatus { get; set; }
        public double AmountToSend { get; set; }
        public double AmountToPaid { get; set; }
        public double FeesAmount { get; set; }
        public CustomerWallet Agent { get; set; }
        public CustomerWallet CustomerWallet { get; set; }
       
        public Ulid? FkIdFeeSend { get; set; }
        public FeeSend? FeeSend { get; set; }
        public Ulid? FkIdPaymentMode { get; set; }
        public PaymentMode? PaymentModeObj { get; set; }
        
     
        [JsonIgnore]
        public ICollection<AccountingOpWallet>? AccountingOpWallets { get; set; }

    }
}
