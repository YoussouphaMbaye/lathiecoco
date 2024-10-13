namespace Lathiecoco.dto
{
    public class BodyInvoiceWalletCashier
    {
        public string PhoneCustomerWallet { get; set; }
        public string PhoneIdentityCustomerWallet { get; set; }
        //public string PhoneCustomerWallet { get; set; }
        public Ulid IdAgent {  get; set; }
        public double AmountToSend { get; set; }
        public double AmountToPaid { get; set; }
    }
}
