namespace Lathiecoco.dto
{
    public class BodyInvoiceSimple
    {

        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderMiddleName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderCountry { get; set; }
        public string SenderCurrency { get; set; }
        public string SenderAddress { get; set; }
        public string SenderIdCard { get; set; }

        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientMiddleName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientCurrency { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientIdCard { get; set; }
        public string RecipientBankCode { get; set; }
        public string RecipientBankName { get; set; }

        
       
        public double AmountToSend { get; set; }
       
        public string PaymentMode { get; set; }

        public string CodePayee { get; set; }
        public string CodeSender { get; set; }
    }
}
