using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class LoginViewModel
    {
        [Required, ShortString, EmailAddress]
        [Display(Name = "ایمیل", Description = "ایمیل خود را وارد کنید")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "کلمه عبور", Description = "کلمه عبور خود را وارد کنید"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}