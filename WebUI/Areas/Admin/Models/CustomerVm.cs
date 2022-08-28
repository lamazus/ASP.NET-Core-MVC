using Domain.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class CustomerVm
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }
}
