using System.ComponentModel.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class VerifyConfirmEmailViewModel
    {
        public string Nonce { get; set; }
        public string FullName { get; set; }

        [Required]
        [Display(Name = "کلمه عبور", Description = "کلمه عبور خود را وارد کنید"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}