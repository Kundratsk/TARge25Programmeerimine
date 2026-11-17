using System.ComponentModel.DataAnnotations;

namespace TARge25shop.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-posti aadress on kohustuslik.")]
        [EmailAddress(ErrorMessage = "Palun sisesta korrektne e-posti aadress.")]
        [Display(Name = "E-posti aadress")]
        public string Email { get; set; }
    }
}