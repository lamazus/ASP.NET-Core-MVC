namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal TotalPrice { get; set; }
        public DateTime DateOfOrder { get; set; } = DateTime.Now;
        public bool IsDelivered { get; set; }
    }
}
