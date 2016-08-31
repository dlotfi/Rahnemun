using System.ComponentModel.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class PasswordResetViewModel
    {
        public string Nonce { get; set; }
        public string FullName { get; set; }

        [Required]
        [Display(Name = "کلمه عبور جدید", Description = "کلمه عبور جدید خود را وارد کنید"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "کلمه عبور جدید و تکرار آن یکسان نیستند.")]
        [Display(Name = "تکرار کلمه عبور جدید", Description = "کلمه عبور جدید وارد شده را تکرار کنید"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}