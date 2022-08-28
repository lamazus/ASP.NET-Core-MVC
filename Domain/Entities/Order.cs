namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<ProductInCart> Products { get; set; } = new List<ProductInCart>();
        public decimal TotalPrice { get; set; }
        public string Commentary { get; set; } = String.Empty;
        public DateTime DateOfOrder { get; set; } = DateTime.Now;
        public DateTime DeliveryDate { get; set;}
        public string DeliveryTime { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsPaid { get; set; }
    }
}
