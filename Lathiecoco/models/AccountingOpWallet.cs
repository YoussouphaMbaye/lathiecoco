using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class AccountingOpWallet:BaseEntity
    {
        [Key]
        public Ulid IdAccountingOperation { get; set; }
        public double Credited { get; set; }
        public double DeBited { get; set; }
        public double NewBalance { get; set; }
        public string PaymentMode { get; set; }
        public Ulid FkIdAccounting { get; set; }
        public Accounting? Accounting { get; set; }
        public Ulid? FkIdInvoice { get; set; }
        public InvoiceWallet? InvoiceWallet { get; set; }
        public Ulid? FkIdBillerInvoice { get; set; }
        public BillerInvoice? BillerInvoice { get; set; }
        public Ulid? FkIdInvoiceWalletAgent { get; set; }
        public InvoiceWalletAgent? InvoiceWalletAgent { get; set; }
        public Ulid? FkIdInvoiceStartupMaster { get; set; }
        public InvoiceStartupMaster? InvoiceStartupMaster { get; set; }

    }
}
