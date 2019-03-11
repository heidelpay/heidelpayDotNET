using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public class Customer : AbstractPayment
    {
        public Customer(string firstName, string lastName)
        {

        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Salutation Salutation { get; set; }
        public string CustomerId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public override string TypeUrl => "customers";
    }

    public enum Salutation { Mr, Ms, Unknown };
}
