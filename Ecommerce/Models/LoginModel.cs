using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class LoginModel
    {


        [Required]
        [Display(Name ="userName")]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
