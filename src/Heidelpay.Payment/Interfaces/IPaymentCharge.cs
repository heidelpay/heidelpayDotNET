namespace Heidelpay.Payment.Interfaces
{
    public interface IPaymentCharge : IPaymentType
    {
        Heidelpay Heidelpay { get; }
    }
}
