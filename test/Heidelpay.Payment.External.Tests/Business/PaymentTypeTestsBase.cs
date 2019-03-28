using Heidelpay.Payment;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.External.Tests.Business
{
    public abstract class PaymentTypeTestsBase
    {
        protected Heidelpay BuildHeidelpay(string privateKey = null)
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(opt =>
            {
                opt.ApiEndpoint = new Uri("https://api.heidelpay.com");
                opt.ApiVersion = "v1";
                opt.ApiKey = privateKey ?? "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n";
            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<Heidelpay>();
        }

        protected Authorization GetAuthorization(string typeId, string customerId = null, string orderId = null, string metadataId = null, string basketId = null)
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

        protected Charge BuildCharge(string typeId = null, string customerId = null, string orderId = null, string metadataId = null, string basketId = null)
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

        protected string GetRandomId()
        {
            return Guid.NewGuid().ToString("B").ToUpper();
        }

        protected async Task<Customer> CreateMaximumCustomer(Heidelpay heidelpay)
        {
            return await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
        }

        protected async Task<Customer> CreateMaximumCustomerSameAddress(Heidelpay heidelpay)
        {
            return await heidelpay.CreateCustomerAsync(GetMaximumCustomerSameAddress(GetRandomId()));
        }

        protected Customer GetMinimumCustomer()
        {
            return new Customer("Rene", "Felder");
        }

        protected Customer GetMaximumCustomerSameAddress(String customerId)
        {
            CoreExtensions.TryParseDateTime("03.10.1974", out DateTime dt);

            return new Customer("Rene", "Felder")
            {
                CustomerId = customerId,
                Salutation = Salutation.Mr,
                Email = "info@heidelpay.com",
                Mobile = "+43676123456",
                BirthDate = dt,
                BillingAddress = TestAddress,
                ShippingAddress = TestAddress,
            };
        }
        protected Customer GetMaximumCustomer(String customerId)
        {
            CoreExtensions.TryParseDateTime("1974-03-10", out DateTime dt);

            return new Customer("Rene", "Felder")
            {
                CustomerId = customerId,
                Salutation = Salutation.Mr,
                Email = "info@heidelpay.com",
                Mobile = "+43676123456",
                BirthDate = dt,
                BillingAddress = TestAddress,
                ShippingAddress = new Address { Name = "Schubert", Street = "Vangerowstraße 18", City = "Heidelberg", Country = "BW", Zip = "69115", State = "DE" },
            };
        }

        protected MetaData TestMetaData { get; } = new MetaData { ["invoice-nr"] = "Rg-2018-11-1", ["shop-id"] = "4711", ["delivery-date"] = "24.12.2018", ["reason"] = "X-mas present" };
        protected Address TestAddress { get; } = new Address { Name = "Mozart", Street = "Grüngasse 16", City = "Vienna", State = "Vienna", Zip = "1010", Country = "AT" };
        protected Card PaymentTypeCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
