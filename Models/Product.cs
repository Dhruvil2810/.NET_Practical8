
using System.ComponentModel.DataAnnotations;

namespace Practical8.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(typeof(decimal), "0.01", "99999999.99",
            ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
    }
}
