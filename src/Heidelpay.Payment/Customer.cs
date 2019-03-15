using System;

namespace Heidelpay.Payment
{
    public class Customer : PaymentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Lastname { get; set; }
        public Salutation Salutation { get; set; }
        public string CustomerId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public Customer(string firstName, string lastName)
        {
            this.Firstname = firstName;
            this.Lastname = lastName;
        }

        public override string TypeUrl => "customers";
    }

    public enum Salutation
    {
        Mr,
        Ms,
        Unknown,
    };
}
