namespace Heidelpay.Payment.Service
{
    internal sealed class PaymentService
    {
        private static readonly string TRANSACTION_TYPE_AUTHORIZATION = "authorize";
        private static readonly string TRANSACTION_TYPE_CHARGE = "charge";
        private static readonly string TRANSACTION_TYPE_CANCEL_AUTHORIZE = "cancel-authorize";
        private static readonly string TRANSACTION_TYPE_CANCEL_CHARGE = "cancel-charge";

        private readonly Heidelpay heidelpay;

        public PaymentService(Heidelpay heidelpay)
        {
            this.heidelpay = heidelpay;
        }
    }
}
