using Ecommerce.Models;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Validators
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ImageRequiredForNewProduct : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = (Product)validationContext.ObjectInstance;

            // Check if this is a new product (Id is 0 or default)
            if (product.Id == 0 && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }


}


