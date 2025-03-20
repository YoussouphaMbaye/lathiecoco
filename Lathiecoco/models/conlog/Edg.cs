namespace Lathiecoco.models.conlog
{
    public class Edg
    {

    }

    public class EdgPayment
    {
        public double montant { get; set; }
        public string? numCompteur { get; set; }
    }
    public class AccountPaymentServicesEdg
    {
        public double amount { get; set; }
        public string?  token { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public string? utilityType { get; set; }
        public string? MeterNumber { get; set; }
        public DateTime paymentDate { get; set; }
        public string? CustomerName { get; set; }
        public string? EnergyCoast { get; set; }
    }

    public class _paymentEDG
    {
        public string? Units { get; set; }
        public string? token { get; set; }
        public string? amountVend { get; set; }
        public string? MeterNumber { get; set; }
        public DateTime payment_date { get; set; }
        public string? unitOfMeasurement { get; set; }
    }

    public class _customer
    {
        public _paymentEDG? payment { get; set; }
        public string? customerName { get; set; }
        public string? customerAdress { get; set; }
        public string? customerPhoneNumber { get; set; }
    }
}
