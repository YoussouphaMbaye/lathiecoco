using System.ComponentModel.DataAnnotations;

namespace  Lathiecoco.models
{
    public class AccountingOpPrincipal:BaseEntity
    {
        [Key]
        public Ulid IdAccountingOpPrincipal { get; set; }
        public double Credited { get; set; }
        public double DeBited { get; set; }
        public double NewBalance { get; set; }
        public string PaymentMode { get; set; }
        public Ulid FkIdAccounting { get; set; }
        public AccountingPrincipal? Accounting { get; set; }
        public Ulid? FkIdInvoiceStartupMaster { get; set; }
        public InvoiceStartupMaster? InvoiceStartupMaster { get; set; }

    }
}
