using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class JoinUsViewModel: ConsultantEditViewModel
    {
        [Required, ShortString, EmailAddress]
        [Display(Name = "ایمیل", Description = "لطفا یک ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "کلمه عبور", Description = "کلمه عبور خود را وارد کنید"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکسان نیستند.")]
        [Display(Name = "تکرار کلمه عبور", Description = "کلمه عبور وارد شده را تکرار کنید"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [UIHint("Disclaimer")]
        public bool Disclaimer { get; set; }


        public string ContactTelNo { get; set; }
        public string ContactTelTitle { get; set; }
    }
}