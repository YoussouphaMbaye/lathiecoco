using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class InvoiceWallet : BaseEntity
    {
        [Key]
        public Ulid IdInvoiceWallet { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceCode2 { get; set; }
        public string PaymentMode { get; set; }
        public Ulid FkIdSender { get; set; }
        public Ulid FkIdRecipient { get; set; }
        public string InvoiceStatus { get; set; }
        public double AmountToSend { get; set; }
        public double AmountToPaid { get; set; }
        public double FeesAmount { get; set; }
        public Ulid? FkIdFeeSend { get; set; }
        public FeeSend? FeeSend { get; set; }
        public Ulid? FkIdPaymentMode { get; set; }
        public PaymentMode? PaymentModeObj { get; set; }
        
        public CustomerWallet CustomerSender { get; set; }
        public CustomerWallet CustomerRecipient { get; set; }
        //public Ulid FkIdCashierPayee { get; set; }
        //public Ulid FkIdCashierSender { get; set; }
        //public Cashier CashierSender { get; set; }
        //public Cashier CashierPayee { get; set; }
        
        [JsonIgnore]
        public ICollection<AccountingOpWallet>? AccountingOpWallets { get; set; }

    }
}
