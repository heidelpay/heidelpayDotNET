using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Heidelpay.Payment
{
    public class Customer : PaymentBase
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Salutation? Salutation { get; set; }
        public string CustomerId { get; set; }

        [JsonConverter(typeof(JsonOnlyDateConverter))]
        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public Customer()
        {

        }

        public override string TypeUrl => "customers";
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Salutation
    {
        [EnumMember(Value="mr")]
        Mr,
        [EnumMember(Value = "ms")]
        Ms,
        [EnumMember(Value = "unknown")]
        Unknown,
    };
}
