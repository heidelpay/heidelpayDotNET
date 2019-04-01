using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class Refund : PaymentBase
    {
        [JsonConstructor]
        internal Refund()
        {

        }

        public override string TypeUrl => string.Empty;
    }
}
