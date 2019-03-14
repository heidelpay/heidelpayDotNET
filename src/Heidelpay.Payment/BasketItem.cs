namespace Heidelpay.Payment
{
    public class BasketItem
    {
        public string BasketItemReferenceId { get; set; }
        public int Quantity { get; set; }
        public int Vat { get; set; }
        public decimal AmountDiscount { get; set; }
        public decimal AmountGross { get; set; }
        public decimal AmountVat { get; set; }
        public decimal AmountPerUnit { get; set; }
        public decimal AmountNet { get; set; }
        public string Unit { get; set; }
        public string Title { get; set; }
    }
}
