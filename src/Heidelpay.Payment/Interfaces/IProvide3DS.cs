namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IProvide3DS
    /// </summary>
    internal interface IProvide3DS
    {
        /// <summary>
        /// Gets a value indicating whether [3ds] is set.
        /// </summary>
        /// <value><c>null</c> if [3ds] contains no value, <c>true</c> if [3ds]; otherwise, <c>false</c>.</value>
        bool? ThreeDs { get; }
    }
}
