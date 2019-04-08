using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Heidelpay.Payment
{
    internal static class HeidelpayRegistry
    {
        /// <summary>
        /// The placeholder charge identifier
        /// </summary>
        private const string PLACEHOLDER_CHARGE_ID = "<chargeId>";

        /// <summary>
        /// The placeholder payment identifier
        /// </summary>
        private const string PLACEHOLDER_PAYMENT_ID = "<paymentId>";

        private const string PAYMENTTYPE_PREFIX = "types/";
        private const string PAYMENT_PREFIX = "payments/<paymentId>/";
        private const string REFUND_PREFIX = PAYMENT_PREFIX + "charges/<chargeId>/cancels";


        static ReadOnlyDictionary<Type, ValueTuple<string, RegistryType>> ResourcePathRegistry { get; } = new ReadOnlyDictionary<Type, ValueTuple<string, RegistryType>>(new Dictionary<Type, ValueTuple<string, RegistryType>>
        {
            [typeof(Basket)] = ("baskets", RegistryType.Root),
            [typeof(Customer)] = ("customers", RegistryType.Root),
            [typeof(MetaData)] = ("metadata", RegistryType.Root),
            [typeof(Payment)] = ("payments", RegistryType.Root),

            [typeof(Authorization)] = ("authorize", RegistryType.Payment),
            [typeof(Cancel)] = ("authorize/cancels", RegistryType.Payment),
            [typeof(Charge)] = ("charges", RegistryType.Payment),
            [typeof(Shipment)] = ("shipments", RegistryType.Payment),

            [typeof(Card)] = ("card", RegistryType.PaymentType),
            [typeof(Eps)] = ("eps", RegistryType.PaymentType),
            [typeof(Giropay)] = ("giropay", RegistryType.PaymentType),
            [typeof(Ideal)] = ("ideal", RegistryType.PaymentType),
            [typeof(Invoice)] = ("invoice", RegistryType.PaymentType),
            [typeof(InvoiceFactoring)] = ("invoice-factoring", RegistryType.PaymentType),
            [typeof(InvoiceGuaranteed)] = ("invoice-guaranteed", RegistryType.PaymentType),
            [typeof(Paypal)] = ("paypal", RegistryType.PaymentType),
            [typeof(Pis)] = ("pis", RegistryType.PaymentType),
            [typeof(Prepayment)] = ("prepayment", RegistryType.PaymentType),
            [typeof(Przelewy24)] = ("przelewy24", RegistryType.PaymentType),
            [typeof(SepaDirectDebit)] = ("sepa-direct-debit", RegistryType.PaymentType),
            [typeof(SepaDirectDebitGuaranteed)] = ("sepa-direct-debit-guaranteed", RegistryType.PaymentType),
            [typeof(Sofort)] = ("sofort", RegistryType.PaymentType),
        });

        static ReadOnlyDictionary<string, Type> PaymentTypeRegistry { get; } = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>
        {
            ["crd"] = typeof(Card),
            ["eps"] = typeof(Eps),
            ["gro"] = typeof(Giropay),
            ["idl"] = typeof(Ideal),
            ["ivc"] = typeof(Invoice),
            ["ivf"] = typeof(InvoiceFactoring),
            ["ivg"] = typeof(InvoiceGuaranteed),
            ["ppl"] = typeof(Paypal),
            ["ppy"] = typeof(Prepayment),
            ["p24"] = typeof(Przelewy24),
            ["sdd"] = typeof(SepaDirectDebit),
            ["ddg"] = typeof(SepaDirectDebitGuaranteed),
            ["sft"] = typeof(Sofort),
            ["pis"] = typeof(Pis),
        });

        public static Type ResolvePaymentType(string typeId)
        {
            var shortTypeId = ExtractTypeShortIdFromTypeId(typeId);

            if(!PaymentTypeRegistry.ContainsKey(shortTypeId))
                throw new PaymentException("Type '" + shortTypeId + "' is currently not supported by the SDK");

            return PaymentTypeRegistry[shortTypeId];
        }


        /// <summary>
        /// Resolves the resource URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.String.</returns>
        public static string ResolveResourceUrl<T>()
        {
            return ResolveResourceUrl(typeof(T));
        }

        /// <summary>
        /// Resolves the payment URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>System.String.</returns>
        public static string ResolvePaymentUrl<T>(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return GetPath(typeof(T))
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .EnsureTrailingSlash();
        }

        /// <summary>
        /// Resolves the refund URL.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>System.String.</returns>
        public static string ResolveRefundUrl(string paymentId, string chargeId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            return REFUND_PREFIX
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .Replace(PLACEHOLDER_CHARGE_ID, chargeId)
                .EnsureTrailingSlash();
        }

        public static string ResolveResourceUrl(Type resourceType)
        {
            return GetPath(resourceType)
                .Replace(PLACEHOLDER_PAYMENT_ID + "/", string.Empty)
                .EnsureTrailingSlash();
        }
        enum RegistryType
        {
            Root,
            Payment,
            PaymentType,
        }

        private static string GetPath(Type type)
        {
            if (!ResourcePathRegistry.ContainsKey(type))
                return string.Empty;

            (string Path, RegistryType Type) entry = ResourcePathRegistry[type];
            var result = entry.Path;

            switch (entry.Type)
            {
                case RegistryType.Payment:
                    result = PAYMENT_PREFIX + result;
                    break;
                case RegistryType.PaymentType:
                    result = PAYMENTTYPE_PREFIX + result;
                    break;
                case RegistryType.Root:
                default:
                    break;
            }

            return result;
        }

        private static string ExtractTypeShortIdFromTypeId(string typeId)
        {
            Check.ThrowIfNullOrEmpty(typeId, nameof(typeId));
            Check.ThrowIfTrue(typeId.Length < 5, "TypeId '" + typeId + "' is invalid");

            return typeId
                .Substring(2, 3)
                .ToLower();
        }
    }
}
