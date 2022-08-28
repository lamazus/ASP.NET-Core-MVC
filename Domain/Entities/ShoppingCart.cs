namespace Domain.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public List<ProductInCart> ProductInCarts { get; set; } = new List<ProductInCart>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
