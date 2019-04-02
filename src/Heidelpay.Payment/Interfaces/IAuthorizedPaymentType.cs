namespace Heidelpay.Payment.Interfaces
{
    public interface IAuthorizedPaymentType : IPaymentType
    {
        Heidelpay Heidelpay { get; }
    }
}
