// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Registry.cs" company="heidelpay GmbH, tieto Austria GmbH">
//     Copyright (c) 2019 Heidelpay GmbH. All rights reserved.
// </copyright>
// ***********************************************************************
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ***********************************************************************

using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Registry.
    /// </summary>
    internal static class Registry
    {
        private const string PLACEHOLDER_CHARGE_ID = "<chargeId>";
        private const string PLACEHOLDER_PAYMENT_ID = "<paymentId>";

        private const string PAYMENTTYPE_PREFIX = "types/";
        private const string PAYMENT_PREFIX = "payments/<paymentId>/";
        private const string REFUND_PREFIX = PAYMENT_PREFIX + "charges/<chargeId>/cancels";


        static ReadOnlyDictionary<Type, ValueTuple<string, string, RegistryType>> ResourcePathRegistry { get; } = new ReadOnlyDictionary<Type, ValueTuple<string, string, RegistryType>>(new Dictionary<Type, ValueTuple<string, string, RegistryType>>
        {
            [typeof(Basket)] = ("baskets", null, RegistryType.Root),
            [typeof(Customer)] = ("customers", null, RegistryType.Root),
            [typeof(MetaData)] = ("metadata", null, RegistryType.Root),
            [typeof(Payment)] = ("payments", null, RegistryType.Root),

            [typeof(Authorization)] = ("authorize", null, RegistryType.Payment),
            [typeof(Cancel)] = ("authorize/cancels", null, RegistryType.Payment),
            [typeof(Charge)] = ("charges", null, RegistryType.Payment),
            [typeof(Shipment)] = ("shipments", null, RegistryType.Payment),

            [typeof(Card)] = ("card", "crd", RegistryType.PaymentType),
            [typeof(Eps)] = ("eps", "eps", RegistryType.PaymentType),
            [typeof(Giropay)] = ("giropay", "gro", RegistryType.PaymentType),
            [typeof(Ideal)] = ("ideal", "idl", RegistryType.PaymentType),
            [typeof(Invoice)] = ("invoice", "ivc", RegistryType.PaymentType),
            [typeof(InvoiceFactoring)] = ("invoice-factoring", "ivf", RegistryType.PaymentType),
            [typeof(InvoiceGuaranteed)] = ("invoice-guaranteed", "ivg", RegistryType.PaymentType),
            [typeof(Paypal)] = ("paypal", "ppl", RegistryType.PaymentType),
            [typeof(Pis)] = ("pis", "pis", RegistryType.PaymentType),
            [typeof(Prepayment)] = ("prepayment", "ppy", RegistryType.PaymentType),
            [typeof(Przelewy24)] = ("przelewy24", "p24", RegistryType.PaymentType),
            [typeof(SepaDirectDebit)] = ("sepa-direct-debit", "sdd", RegistryType.PaymentType),
            [typeof(SepaDirectDebitGuaranteed)] = ("sepa-direct-debit-guaranteed", "ddg", RegistryType.PaymentType),
            [typeof(Sofort)] = ("sofort", "sft", RegistryType.PaymentType),
            [typeof(Alipay)] = ("alipay", "ali", RegistryType.PaymentType),
            [typeof(WeChatPay)] = ("wechatpay", "wcp", RegistryType.PaymentType),
            [typeof(Applepay)] = ("applepay", "apl", RegistryType.PaymentType),
        });

        static ReadOnlyDictionary<string, Type> PaymentTypeRegistry { get; } = new ReadOnlyDictionary<string, Type>(ResourcePathRegistry.Keys
            .Where(x => ResourcePathRegistry[x].Item3 == RegistryType.PaymentType)
            .ToDictionary(x => ResourcePathRegistry[x].Item2, x => x));

        /// <summary>
        /// Resolves the type of the payment.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Type.</returns>
        /// <exception cref="PaymentException">Type '" + shortTypeId + "' is currently not supported by the SDK</exception>
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

            (string Path, string ShortName, RegistryType Type) entry = ResourcePathRegistry[type];
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
