using System.ComponentModel.DataAnnotations;

namespace API.Attributes
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password == null)
            {
                return new ValidationResult("Password is required.");
            }

            if (password.Length < 4 || password.Length > 32)
            {
                return new ValidationResult("Password must be 4 to 32 characters long.");
            }

            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult("Password must contain at least one digit.");
            }

            if (!password.Any(char.IsLower))
            {
                return new ValidationResult("Password must contain at least one lowercase letter.");
            }

            if (!password.Any(char.IsUpper))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            return ValidationResult.Success;
        }
        
    }
}