using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;

namespace Heidelpay.Payment.Payment
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Paypage : PaymentTransactionBase
    {
        /// <summary>
        /// Gets or sets the logo image.
        /// </summary>
        /// <value>
        /// The logo image.
        /// </value>
        public string LogoImage { get; set; }

        /// <summary>
        /// Gets or sets the basket image.
        /// </summary>
        /// <value>
        /// The basket image.
        /// </value>
        public string BasketImage { get; set; }

        /// <summary>
        /// Gets or sets the full page image.
        /// </summary>
        /// <value>
        /// The full page image.
        /// </value>
        public string FullPageImage { get; set; }

        /// <summary>
        /// Gets or sets the name of the shop.
        /// </summary>
        /// <value>
        /// The name of the shop.
        /// </value>
        public string ShopName { get; set; }

        /// <summary>
        /// Gets or sets the description main.
        /// </summary>
        /// <value>
        /// The description main.
        /// </value>
        public string DescriptionMain { get; set; }

        /// <summary>
        /// Gets or sets the description small.
        /// </summary>
        /// <value>
        /// The description small.
        /// </value>
        public string DescriptionSmall { get; set; }

        /// <summary>
        /// Gets or sets the terms and condition URI.
        /// </summary>
        /// <value>
        /// The terms and condition URI.
        /// </value>
        public Uri TermsAndConditionUri { get; set; }

        /// <summary>
        /// Gets or sets the privacy policy URI.
        /// </summary>
        /// <value>
        /// The privacy policy URI.
        /// </value>
        public Uri PrivacyPolicyUri { get; set; }

        /// <summary>
        /// Gets or sets the impressum URI.
        /// </summary>
        /// <value>
        /// The impressum URI.
        /// </value>
        public Uri ImpressumUri { get; set; }

        /// <summary>
        /// Gets or sets the help URI.
        /// </summary>
        /// <value>
        /// The help URI.
        /// </value>
        public Uri HelpUri { get; set; }

        /// <summary>
        /// Gets or sets the contact URI.
        /// </summary>
        /// <value>
        /// The contact URI.
        /// </value>
        public Uri ContactUri { get; set; }

        [JsonConstructor]
        internal Paypage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paypage"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        internal Paypage(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }
    }
}
