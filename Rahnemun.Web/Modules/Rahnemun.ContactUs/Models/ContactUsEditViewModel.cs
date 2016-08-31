using System;
using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.ContactUs
{
    public class ContactUsEditViewModel
    {
        [Required, MaxStringLength(50)]
        [Display(Name = "نام شما", Description = "نام خود را وارد نمایید")]
        public string Name { get; set; }

        [Required, ShortString, EmailAddress]
        [Display(Name = "ایمیل", Description = "لطفا یک ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "موضوع")]
        public CustomerMessageSubject Subject { get; set; }

        [Required]
        [Display(Name = "پیام", Description = "متن پیام خود را درج نمایید"), DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Required]
        [Display(Name = "کد امنیتی"), UIHint("Captcha")]
        public string Captcha { get; set; }
        public Guid CaptchaId { get; set; }

        public string AddUrl { get; set; }
        public bool IsUserLoggedin { get; set; }
        public string ResultMessage { get; set; }
        public string ResultMessageTitle { get; set; }
        public string ContactTelNo { get; set; }
        public string ContactTelTitle { get; set; }
    }
}
