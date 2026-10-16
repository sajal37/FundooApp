
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must contain at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Range(6000000000, 9999999999,
    ErrorMessage = "Contact number must be a valid 10-digit mobile number")]
        public long ContactNo { get; set; }
    }
}