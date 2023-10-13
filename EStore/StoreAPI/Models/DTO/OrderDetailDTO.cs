namespace StoreAPI.Models.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public OrderDetail GetOrderDetail()
        {
            return new OrderDetail()
            {
                OrderDetailId = OrderDetailId,
                ProductId = ProductId,
                OrderId = OrderId,
                UnitPrice = UnitPrice,
                Quantity = Quantity,
                Discount = Discount
            };
        }
    }
}
