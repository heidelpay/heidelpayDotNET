namespace Heidelpay.Payment.Communication.Internal
{
    internal class Amount
    {
        public decimal Total { get; set; }
        public decimal Charged { get; set; }
        public decimal Canceled { get; set; }
        public decimal Remaining { get; set; }
    }
}
