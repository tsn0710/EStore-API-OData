namespace StoreClient.Models
{
    public class OrderView
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int? Freight { get; set; }
        public int MaxNo { get; set; } = 0;
        public virtual ICollection<OrderDetailView> OrderDetailsView { get; set; }
    }
}
