// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="Customer.cs" company="Heidelpay">
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
        /// 
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
        /// <param name="companyName">Name of the company.</param>
        public Customer(string companyName)
        {
            this.CompanyName = companyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer(string firstName, string lastName)
        {
            this.Firstname = firstName;
            this.Lastname = lastName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        [JsonConstructor]
        public Customer()
        {
            
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [JsonProperty(PropertyName = "company")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company information.
        /// </summary>
        /// <value>
        /// The company information.
        /// </value>
        public CompanyInfo CompanyInfo { get; set; }
    }

    /// <summary>
    /// Enum Salutation
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Salutation
    {
        /// <summary>
        /// Mr
        /// </summary>
        [EnumMember(Value="mr")]
        Mr,
        /// <summary>
        /// Ms
        /// </summary>
        [EnumMember(Value = "ms")]
        Ms,
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown,
    };
}
