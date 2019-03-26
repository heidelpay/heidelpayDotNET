using Heidelpay.Payment.Communication.Internal;

namespace Heidelpay.Payment
{
    public class Cancel : PaymentBase
    {
        public decimal Amount { get; set; }
        public Processing Processing { get; set; } = new Processing();

        public Cancel()
        {
        }

        public Cancel(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public override string TypeUrl => "payments/<paymentId>/authorize/cancels";
    }
}
