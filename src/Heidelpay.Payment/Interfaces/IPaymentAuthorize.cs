namespace Heidelpay.Payment.Interfaces
{
    public interface IPaymentAuthorize : IPaymentType
    {
        Heidelpay Heidelpay { get; }
    }
}
