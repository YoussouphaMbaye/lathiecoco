namespace Lathiecoco.dto
{
    public class BodyPostInvoiceWallet
    {
     
        public string PhoneSender { get; set; }
        public string PhoneIdentitySender { get; set; }
        public string PhoneRecipient { get; set; }
        public string PhoneIdentityecipient { get; set; }
        public string CountryDestination { get; set; }
        public string CountryOrigin { get; set; }
        public string CurrencyDestination { get; set; }
        public string CurrencyOrigin { get; set; }
        //public string CodePayee { get; set; }
        public double AmountToSend { get; set; }
        public double AmountToPaid { get; set; }
    }
}
