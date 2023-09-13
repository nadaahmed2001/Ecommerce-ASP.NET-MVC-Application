using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least one letter, one number, and one special character.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, int.MaxValue, ErrorMessage = "Age must be 16 or greater")]
        public int? Age { get; set; }

        public string Role { get; set; }
    }
}
