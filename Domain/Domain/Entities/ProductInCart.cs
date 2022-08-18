namespace Domain.Entities
{
    public class ProductInCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Guid? ShoppingCartId { get; set; } 
        public ShoppingCart? ShoppingCart { get; set; } = null;
        public int Amount { get; set; }
    }
}
