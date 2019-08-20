using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    /// <summary>
    /// HirePurchaseRatePlan
    /// </summary>
    public sealed class HirePurchaseRatePlan : IRestResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HirePurchaseRatePlan"/> class.
        /// </summary>
        [JsonConstructor]
        internal HirePurchaseRatePlan()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the bic.
        /// </summary>
        /// <value>
        /// The bic.
        /// </value>
        public string Bic { get; set; }

        /// <summary>
        /// Gets or sets the iban.
        /// </summary>
        /// <value>
        /// The iban.
        /// </value>
        public string Iban { get; set; }

        /// <summary>
        /// Gets or sets the AccountHolder.
        /// </summary>
        /// <value>
        /// The holder.
        /// </value>
        public string AccountHolder { get; set; }

        /// <summary>
        /// Gets the number of rates.
        /// </summary>
        /// <value>
        /// The number of rates.
        /// </value>
        [JsonProperty]
        public int NumberOfRates { get; internal set; }

        /// <summary>
        /// Gets the day of purchase.
        /// </summary>
        /// <value>
        /// The day of purchase.
        /// </value>
        [JsonProperty]
        public DateTime DayOfPurchase { get; internal set; }

        /// <summary>
        /// Gets the invoice date.
        /// </summary>
        /// <value>
        /// The invoice date.
        /// </value>
        [JsonProperty]
        public DateTime InvoiceDate { get; internal set; }

        /// <summary>
        /// Gets the invoice due date.
        /// </summary>
        /// <value>
        /// The invoice due date.
        /// </value>
        [JsonProperty]
        public DateTime InvoiceDueDate { get; internal set; }

        /// <summary>
        /// Gets the total purchase amount.
        /// </summary>
        /// <value>
        /// The total purchase amount.
        /// </value>
        [JsonProperty]
        public decimal TotalPurchaseAmount { get; internal set; }

        /// <summary>
        /// Gets the total interest amount.
        /// </summary>
        /// <value>
        /// The total interest amount.
        /// </value>
        [JsonProperty]
        public decimal TotalInterestAmount { get; internal set; }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        [JsonProperty]
        public decimal TotalAmount { get; internal set; }

        /// <summary>
        /// Gets the effective interest rate.
        /// </summary>
        /// <value>
        /// The effective interest rate.
        /// </value>
        [JsonProperty]
        public decimal EffectiveInterestRate { get; internal set; }

        /// <summary>
        /// Gets the nominal interest rate.
        /// </summary>
        /// <value>
        /// The nominal interest rate.
        /// </value>
        [JsonProperty]
        public decimal NominalInterestRate { get; internal set; }

        /// <summary>
        /// Gets the fee first rate.
        /// </summary>
        /// <value>
        /// The fee first rate.
        /// </value>
        [JsonProperty]
        public decimal FeeFirstRate { get; internal set; }

        /// <summary>
        /// Gets the fee per rate.
        /// </summary>
        /// <value>
        /// The fee per rate.
        /// </value>
        [JsonProperty]
        public decimal FeePerRate { get; internal set; }

        /// <summary>
        /// Gets the monthly rate.
        /// </summary>
        /// <value>
        /// The monthly rate.
        /// </value>
        [JsonProperty]
        public decimal MonthlyRate { get; internal set; }

        /// <summary>
        /// Gets the last rate.
        /// </summary>
        /// <value>
        /// The last rate.
        /// </value>
        [JsonProperty]
        public decimal LastRate { get; internal set; }

        /// <summary>
        /// Gets the rate list.
        /// </summary>
        /// <value>
        /// The rate list.
        /// </value>
        [JsonProperty]
        public IEnumerable<HirePurchaseRate> RateList { get; internal set; }

    }

    internal sealed class HirePurchaseRatePlanList : IRestResource
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public ICollection<HirePurchaseRatePlan> Entity { get; set; }

        public HirePurchaseRatePlanList()
        {
            Entity = new List<HirePurchaseRatePlan>();
        }
    }
}
