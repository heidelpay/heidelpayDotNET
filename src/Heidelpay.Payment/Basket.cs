﻿using System.Collections.Generic;

namespace Heidelpay.Payment
{
    public class Basket : PaymentBase
    {
        public string Id { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AmountTotalDiscount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderId { get; set; }
        public string Note { get; set; }

        private List<BasketItem> basketItems = new List<BasketItem>();
        public IEnumerable<BasketItem> BasketItems { get => basketItems; } 

        public Basket()
        {
        }

        public Basket(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public void AddBasketItem(BasketItem item)
        {
            basketItems.Add(item);
        }

        public override string TypeUrl => "baskets";
    }
}