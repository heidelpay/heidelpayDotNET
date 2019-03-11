using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public class Address
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
