
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must contain at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}