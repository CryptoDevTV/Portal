namespace Portal.Web.ViewModels.Payments
{
    public class Data
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public long InvoiceTime { get; set; }
        public long ExpirationTime { get; set; }
        public long CurrentTime { get; set; }
        public string BtcPaid { get; set; }
        public string BtcDue { get; set; }
        public double Rate { get; set; }
        public bool ExceptionStatus { get; set; }
        public BuyerFields BuyerFields { get; set; }
        public Payment PaymentSubtotals { get; set; }
        public Payment PaymentTotals { get; set; }
        public string TransactionCurrency { get; set; }
        public string AmountPaid { get; set; }
        public ExchangeRates ExchangeRates { get; set; }
    }
}