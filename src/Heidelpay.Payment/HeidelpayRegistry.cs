using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Heidelpay.Payment
{
    internal static class HeidelpayRegistry
    {
        internal const string PAYMENT_PREFIX = "payments/<paymentId>/";
        internal const string PAYMENTTYPE_PREFIX = "types/";

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

        public static string GetPath(Type type)
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
    }
}
