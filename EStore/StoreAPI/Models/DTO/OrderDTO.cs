namespace StoreAPI.Models.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public string OrderDate { get; set; }
        public string? RequiredDate { get; set; }
        public string ShippedDate { get; set; }
        public int? Freight { get; set; }
        public Order GetOrder()
        {
            if (RequiredDate == null)
            {
                return new Order()
                {
                    OrderId = OrderId,
                    MemberId = MemberId,
                    OrderDate = DateTime.Parse(OrderDate),
                    RequiredDate = null,
                    ShippedDate = DateTime.Parse(ShippedDate),
                    Freight = Freight
                };
            }
            else
            {
                return new Order()
                {
                    OrderId = OrderId,
                    MemberId = MemberId,
                    OrderDate = DateTime.Parse(OrderDate),
                    RequiredDate = DateTime.Parse(RequiredDate),
                    ShippedDate = DateTime.Parse(ShippedDate),
                    Freight = Freight
                };
            }
        }
    }
}
