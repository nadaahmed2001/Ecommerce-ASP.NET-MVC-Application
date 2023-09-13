using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        // Define the foreign key relationships to User and Product
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Required]
        public int? Quantity { get; set; }
    }
}
