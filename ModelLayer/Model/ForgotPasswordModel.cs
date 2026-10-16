
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}