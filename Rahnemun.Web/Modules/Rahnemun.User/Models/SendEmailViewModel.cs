using System;
using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class SendEmailViewModel
    {
        public SendEmailPurpose Purpose { get; set; }

        [Required, ShortString, EmailAddress]
        [Display(Name = "ایمیل", Description = "آدرس ایمیلی که با آن ثبت نام کرده اید را وارد کنید")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "کد امنیتی"), UIHint("Captcha")]
        public string Captcha { get; set; }
        public Guid CaptchaId { get; set; }
    }

    public enum SendEmailPurpose
    {
        ConfirmEmail,
        ResetPassword
    }
}