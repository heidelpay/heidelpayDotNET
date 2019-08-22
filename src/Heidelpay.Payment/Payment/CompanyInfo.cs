using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyInfo
    {
        /// <summary>
        /// Gets or sets the type of the registration.
        /// </summary>
        /// <value>
        /// The type of the registration.
        /// </value>
        [JsonProperty]
        public RegistrationType RegistrationType { get; internal set; }

        /// <summary>
        /// Gets or sets the commercial sector.
        /// </summary>
        /// <value>
        /// The commercial sector.
        /// </value>
        public CommercialSector CommercialSector { get; set; }

        /// <summary>
        /// Gets or sets the commercial register number.
        /// </summary>
        /// <value>
        /// The commercial register number.
        /// </value>
        public string CommercialRegisterNumber { get; set; }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        [JsonProperty]
        internal string Function { get; set; }

        [JsonConstructor]
        internal CompanyInfo()
        {

        }

        /// <summary>
        /// Creates new registered.
        /// </summary>
        /// <returns></returns>
        public static CompanyInfo BuildRegistered()
        {
            return new CompanyInfo { RegistrationType = RegistrationType.REGISTERED };
        }

        /// <summary>
        /// Creates new unregistered.
        /// </summary>
        /// <returns></returns>
        public static CompanyInfo BuildUnregistered()
        {
            return new CompanyInfo { RegistrationType = RegistrationType.NOT_REGISTERED, Function = "OWNER" };
        }

    }
}
