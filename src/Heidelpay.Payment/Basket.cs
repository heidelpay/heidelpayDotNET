using Heidelpay.Payment.Interfaces;
using System.Collections.Generic;

namespace Heidelpay.Payment
{
    public class Basket : IRestResource
    {
        public string Id { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AmountTotalDiscount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderId { get; set; }
        public string Note { get; set; }

        readonly List<BasketItem> basketItems = new List<BasketItem>();
        public IEnumerable<BasketItem> BasketItems { get => basketItems; } 

        public Basket()
        {
        }

        public void AddBasketItem(BasketItem item)
        {
            basketItems.Add(item);
        }

        public string TypeUrl => "baskets";
    }
}
