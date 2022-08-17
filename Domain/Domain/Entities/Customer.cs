using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } 
        [RegularExpression("^((+7|7|8)+([0-9]){10})$")]
        public string TelephoneNumber { get; set; }
        [RegularExpression("^[-w.]+@([A-z0-9][-A-z0-9]+.)+[A-z]{2,4}$")]
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();

    }
}
