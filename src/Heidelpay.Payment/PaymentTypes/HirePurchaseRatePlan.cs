using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// HirePurchaseRatePlan
    /// </summary>
    public sealed class HirePurchaseRatePlan : PaymentTypeBase, IAuthorizedPaymentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HirePurchaseRatePlan"/> class.
        /// </summary>
        [JsonConstructor]
        internal HirePurchaseRatePlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HirePurchaseRatePlan"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client.</param>
        public HirePurchaseRatePlan(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Builds a payment type configuration based on this instance.
        /// </summary>
        /// <returns></returns>
        public Action<HirePurchaseRatePlan> PaymentTypeConfig()
        {
            return new Action<HirePurchaseRatePlan>(x =>
            {
                CopyPlan(this, x);
            });
        }

        /// <summary>
        /// Copies the plan.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyPlan(HirePurchaseRatePlan source, HirePurchaseRatePlan target)
        {
            if(!string.IsNullOrEmpty(target.Id))
                throw new InvalidOperationException("HirePurchaseRatePlan has an Id set. CopyPlan can only be called without Id.");

            target.AccountHolder = source.AccountHolder;
            target.Bic = source.Bic;
            target.DayOfPurchase = source.DayOfPurchase;
            target.EffectiveInterestRate = source.EffectiveInterestRate;
            target.FeeFirstRate = source.FeeFirstRate;
            target.FeePerRate = source.FeePerRate;
            target.Iban = source.Iban;
            target.InvoiceDate = source.InvoiceDate;
            target.InvoiceDueDate = source.InvoiceDueDate;
            target.LastRate = source.LastRate;
            target.MonthlyRate = source.MonthlyRate;
            target.NominalInterestRate = source.NominalInterestRate;
            target.NumberOfRates = source.NumberOfRates;
            target.Recurring = source.Recurring;
            target.TotalAmount = source.TotalAmount;
            target.TotalInterestAmount = source.TotalInterestAmount;
            target.TotalPurchaseAmount = source.TotalPurchaseAmount;
            target.Rates = source.Rates.Select(y => new HirePurchaseRate
            {
                AmountOfRepayment = y.AmountOfRepayment,
                Rate = y.Rate,
                RateIndex = y.RateIndex,
                TotalRemainingAmount = y.TotalRemainingAmount,
                Type = y.Type,
                Ultimo = y.Ultimo,
            }).ToList();
        }

        /// <summary>
        /// The payment configuration
        /// </summary>

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
        public int? NumberOfRates { get; set; }

        /// <summary>
        /// Gets the day of purchase.
        /// </summary>
        /// <value>
        /// The day of purchase.
        /// </value>
        [JsonProperty]
        [JsonConverter(typeof(JsonOnlyDateConverter))]
        public DateTime? DayOfPurchase { get; set; }

        /// <summary>
        /// Gets the invoice date.
        /// </summary>
        /// <value>
        /// The invoice date.
        /// </value>
        [JsonProperty]
        [JsonConverter(typeof(JsonOnlyDateConverter))]
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// Gets the invoice due date.
        /// </summary>
        /// <value>
        /// The invoice due date.
        /// </value>
        [JsonProperty]
        [JsonConverter(typeof(JsonOnlyDateConverter))]
        public DateTime? InvoiceDueDate { get; set; }

        /// <summary>
        /// Gets the total purchase amount.
        /// </summary>
        /// <value>
        /// The total purchase amount.
        /// </value>
        [JsonProperty]
        public decimal? TotalPurchaseAmount { get; set; }

        /// <summary>
        /// Gets the total interest amount.
        /// </summary>
        /// <value>
        /// The total interest amount.
        /// </value>
        [JsonProperty]
        public decimal? TotalInterestAmount { get; set; }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        [JsonProperty]
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// Gets the effective interest rate.
        /// </summary>
        /// <value>
        /// The effective interest rate.
        /// </value>
        [JsonProperty]
        public decimal? EffectiveInterestRate { get; set; }

        /// <summary>
        /// Gets the nominal interest rate.
        /// </summary>
        /// <value>
        /// The nominal interest rate.
        /// </value>
        [JsonProperty]
        public decimal? NominalInterestRate { get; set; }

        /// <summary>
        /// Gets the fee first rate.
        /// </summary>
        /// <value>
        /// The fee first rate.
        /// </value>
        [JsonProperty]
        public decimal? FeeFirstRate { get; set; }

        /// <summary>
        /// Gets the fee per rate.
        /// </summary>
        /// <value>
        /// The fee per rate.
        /// </value>
        [JsonProperty]
        public decimal? FeePerRate { get; set; }

        /// <summary>
        /// Gets the monthly rate.
        /// </summary>
        /// <value>
        /// The monthly rate.
        /// </value>
        [JsonProperty]
        public decimal? MonthlyRate { get; set; }

        /// <summary>
        /// Gets the last rate.
        /// </summary>
        /// <value>
        /// The last rate.
        /// </value>
        [JsonProperty]
        public decimal? LastRate { get; set; }

        /// <summary>
        /// Gets the rate list.
        /// </summary>
        /// <value>
        /// The rate list.
        /// </value>
        [JsonProperty(PropertyName = "installmentRates")]
        public IEnumerable<HirePurchaseRate> Rates { get; internal set; }

        /// <summary>
        /// Gets or sets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
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
