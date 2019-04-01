using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.External.Tests
{
    public static class InternalsHelper
    {
        public static Authorization GetAuthorization(string typeId, string customerId = null, string orderId = null, string metadataId = null, string basketId = null)
        {
            return new Authorization
            {
                Amount = 10m,
                Currency = "EUR",
                ReturnUrl = new Uri("https://www.heidelpay.com"),
                OrderId = orderId,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                    MetadataId = metadataId,
                    BasketId = basketId,
                }
            };
        }

        public static Charge BuildCharge(string typeId = null, string customerId = null, string orderId = null, string metadataId = null, string basketId = null)
        {
            return new Charge
            {
                Amount = decimal.One,
                Currency = "EUR",
                ReturnUrl = new Uri("https://www.google.at"),
                OrderId = orderId,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                    MetadataId = metadataId,
                    BasketId = basketId,
                }
            };
        }
    }
}
