namespace Heidelpay.Payment
{
    public class PaymentError
    {
        public string MerchantMessage { get; internal set; }
        public string CustomerMessage { get; internal set; }
        public string Code { get; internal set; }

        public PaymentError()
        {

        }

        public PaymentError(string merchantMessage, string customerMessage, string code)
        {
            MerchantMessage = merchantMessage;
            CustomerMessage = customerMessage;
            Code = code;
        }

        public override string ToString()
        {
            return $"{{merchantMessage: {MerchantMessage}, customerMessage: {CustomerMessage}, code: {Code}}}";
        }
    }
}
