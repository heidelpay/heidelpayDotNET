// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Customer.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for Customer toghether with billingAddress.
    /// 
    /// firstname and lastname are mandatory to create a new Customer.
    /// 
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    public class Customer : IRestResource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>The firstname.</value>
        public string Firstname { get; set; }
        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>The lastname.</value>
        public string Lastname { get; set; }
        /// <summary>
        /// Gets or sets the salutation.
        /// </summary>
        /// <value>The salutation.</value>
        public Salutation? Salutation { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>The birth date.</value>
        [JsonConverter(typeof(JsonOnlyDateConverter))]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the mobile.
        /// </summary>
        /// <value>The mobile.</value>
        public string Mobile { get; set; }
        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>The billing address.</value>
        public Address BillingAddress { get; set; }
        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        /// <value>The shipping address.</value>
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer(string firstName, string lastName)
        {
            this.Firstname = firstName;
            this.Lastname = lastName;
        }

        [JsonConstructor]
        internal Customer()
        {

        }
    }

    /// <summary>
    /// Enum Salutation
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Salutation
    {
        /// <summary>
        /// The mr
        /// </summary>
        [EnumMember(Value="mr")]
        Mr,
        /// <summary>
        /// The ms
        /// </summary>
        [EnumMember(Value = "ms")]
        Ms,
        /// <summary>
        /// The unknown
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown,
    };
}
