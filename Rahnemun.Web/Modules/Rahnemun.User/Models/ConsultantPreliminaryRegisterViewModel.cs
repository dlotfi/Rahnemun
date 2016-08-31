﻿using System;
using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.User.Models
{
    public class ConsultantPreliminaryRegisterViewModel
    {
        [Required, MaxStringLength(30)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required, MaxStringLength(30)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required, ShortString, EmailAddress]
        [Display(Name = "ایمیل", Description = "لطفا یک ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "کلمه عبور", Description = "کلمه عبور خود را وارد کنید"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = " کلمه عبور و تکرار آن یکسان نیستند")]
        [Display(Name = "تکرار کلمه عبور", Description = "کلمه عبور وارد شده را تکرار کنید"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "کد امنیتی"), UIHint("Captcha")]
        public string Captcha { get; set; }
        public Guid CaptchaId { get; set; }

        [Display(Name = "مایلم خبرنامۀ رهنمون را دریافت نمایم.", Description = "در صورت عدم تمایل به دریافت خبرنامه، گزینه را غیر فعال نمایید.")]
        public bool SubscribedToNewsletter { get; set; }

        [UIHint("Disclaimer")]
        public bool Disclaimer { get; set; }

        public string ReturnUrl { get; set; }
    }
}