using System.ComponentModel.DataAnnotations;
using API.Attributes;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public required string Email {get; set;}

        [Required]
        [PasswordComplexity]
        public required string Password {get; set;}

        [Required]
        public required string UserName {get; set;}
    }
}