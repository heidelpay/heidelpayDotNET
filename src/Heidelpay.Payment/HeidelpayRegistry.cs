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

        /// <summary>
        /// The refund URL
        /// </summary>
        private const string REFUND_URL = "payments/<paymentId>/charges/<chargeId>/cancels";

        private const string PAYMENT_PREFIX = "payments/<paymentId>/";
        private const string PAYMENTTYPE_PREFIX = "types/";

        static ReadOnlyDictionary<Type, ValueTuple<string, RegistryType>> InternalRegistry { get; } = new ReadOnlyDictionary<Type, ValueTuple<string, RegistryType>>(new Dictionary<Type, ValueTuple<string, RegistryType>>
            {
                [typeof(Basket)]   = ("baskets", RegistryType.Root),
                [typeof(Customer)] = ("customers", RegistryType.Root),
                [typeof(MetaData)] = ("metadata", RegistryType.Root),
                [typeof(Payment)]  = ("payments", RegistryType.Root),

                [typeof(Authorization)] = ("authorize", RegistryType.Payment),
                [typeof(Cancel)]        = ("authorize/cancels", RegistryType.Payment),
                [typeof(Charge)]        = ("charges", RegistryType.Payment ),
                [typeof(Shipment)]      = ("shipments", RegistryType.Payment),

                [typeof(Card)]              = ("card", RegistryType.PaymentType ),
                [typeof(Eps)]               = ("eps", RegistryType.PaymentType ),
                [typeof(Giropay)]           = ("giropay", RegistryType.PaymentType ),
                [typeof(Ideal)]             = ("ideal", RegistryType.PaymentType ),
                [typeof(Invoice)]           = ("invoice", RegistryType.PaymentType ),
                [typeof(InvoiceFactoring)]  = ("invoice-factoring", RegistryType.PaymentType ),
                [typeof(InvoiceGuaranteed)] = ("invoice-guaranteed", RegistryType.PaymentType ),
                [typeof(Paypal)]            = ("paypal", RegistryType.PaymentType ),
                [typeof(Pis)]               = ("pis", RegistryType.PaymentType ),
                [typeof(Prepayment)]        = ("prepayment", RegistryType.PaymentType ),
                [typeof(Przelewy24)]        = ("przelewy24", RegistryType.PaymentType ),
                [typeof(SepaDirectDebit)]   = ("sepa-direct-debit", RegistryType.PaymentType ),
                [typeof(SepaDirectDebitGuaranteed)] = ("sepa-direct-debit-guaranteed", RegistryType.PaymentType ),
                [typeof(Sofort)]            = ("sofort", RegistryType.PaymentType ),
            });

        enum RegistryType
        {
            Root,
            Payment,
            PaymentType,
        }

        private static string GetPath(Type type)
        {
            if (!InternalRegistry.ContainsKey(type))
                return string.Empty;

            (string Path, RegistryType Type) entry = InternalRegistry[type];
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

            return REFUND_URL
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
    }
}
