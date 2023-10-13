namespace StoreClient.Models
{
    public class OrderDetailView
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string isDeleted { get; set; } = "f";
    }
}
