namespace Heidelpay.Payment.Interfaces
{
    public interface IChargeablePaymentType : IPaymentType
    {
        Heidelpay Heidelpay { get; }
    }
}
