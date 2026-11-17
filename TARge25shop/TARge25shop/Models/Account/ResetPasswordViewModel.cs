using System.ComponentModel.DataAnnotations;

namespace TARge25shop.Models.Account
{
    public class ResetPasswordViewModel
    {

        [EmailAddress]
        string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmationPassword { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
