using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class CustomerWallet:BaseEntity
    {
        [Key]
        public Ulid IdCustomerWallet { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Phone { get; set; }
        public string phoneIdentity { get; set; }
        [JsonIgnore]
        public string PinNumber { get; set; }
        [JsonIgnore]
        public string? PinTemp { get; set; }
        public string Profile { get; set; }
        public string? PhoneBrand { get; set; }
        public string? Address { get; set; }
        public Boolean IsBlocked { get; set; }=false;
        public Boolean IsActive { get; set; } = true;
        public Ulid FkIdAccounting { get; set; }
        public string Code { get; set; }
        
        public Accounting? Accounting   { get; set; }

        
        [JsonIgnore]
        public ICollection<InvoiceWallet>? InvoiceWalletRecipeients { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceStartupMaster>? InvoiceStartupMasters { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWallet>? InvoiceWalletSenders { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWalletAgent>? InvoiceWalletAgents { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceWalletAgent>? InvoiceWalletAgentAgents { get; set; }
        [JsonIgnore]
        public ICollection<BillerInvoice>? BillerInvoices { get; set; }

    }
}
