using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Validators;

namespace Ecommerce.Models
{
    public class Product
    {
        public Product() { 
            Carts = new List<Cart>();
        }

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityAvailable { get; set; }

        [ImageRequiredForNewProduct(ErrorMessage = "Image is required for new products.")]
        public string? Image { get; set; }

        [ForeignKey("category")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }

}
