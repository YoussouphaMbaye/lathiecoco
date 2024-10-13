using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class PaymentMode:BaseEntity
     {
        [Key]
        public Ulid IdPaymentMode {  get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public bool status { get; set; }
        [JsonIgnore]
        public ICollection<FeeSend>? FeeSends { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWallet>? InvoiceWallets { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWalletAgent>? InvoiceWalletAgents { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceStartupMaster>? InvoiceStartupMasters { get; set; }
        [JsonIgnore]
        public ICollection<BillerInvoice>? BillerInvoices { get; set; }
    }
}
