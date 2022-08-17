namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public int NumberOfOrders { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
