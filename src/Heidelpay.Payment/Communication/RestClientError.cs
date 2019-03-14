namespace Heidelpay.Payment.Communication
{
    public class RestClientError
    {
        public string Code { get; set; }
        public string MerchantMessage { get; set; }
        public string CustomerMessage { get; set; }
        public string Customer { get; set; }
    }
}
