using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Rahnemun.Common;

namespace Rahnemun.User.Models
{
    [AdditionalMetadata("MarkRequiredFields", true)]
    public class ConsultantEditViewModel : UserEditViewModel
    {
        [Required, MaxStringLength(50)]
        [Display(Name = "عنوان تخصصی", Description = "این عنوان همراه نام شما نمایش داده می شود")]
        public string Title { get; set; }

        [Required, CreditCardAttribute]
        [Display(Name = "شماره کارت بانکی", Description = "شماره 16 رقمی کارت بانکی بدون فاصله")]
        public string BankCardNo { get; set; }

        [FixedStringLength(30)]
        [Display(Name = "شماره حساب بانکی", Description = "شماره حساب بانک")]
        public string BankAccountNo { get; set; }

        [FixedStringLength(50)]
        [Display(Name = "نام بانک", Description = "بانک مربوط به حساب بانکی فوق")]
        public string BankName { get; set; }

        [Required, MaxStringLength(1000)]
        [Display(Name = "مدارک تحصیلی", Description = "جزییات مدارک تحصیلی با ذکر دانشگاه و سال اخذ "), DataType(DataType.MultilineText)]
        public string Education { get; set; }

        [Required, MaxStringLength(1000)]
        [Display(Name = "سابقه کار", Description = "جزییات سوابق کاری"), DataType(DataType.MultilineText)]
        public string ProfessionalExperience { get; set; }

        [MaxStringLength(30)]
        [Display(Name = "شماره مجوز", Description = "شماره مجوز حرفه ای (کد نظام پزشکی، شماره پروانه وکالت یا ...)")]
        public string LicenseNumber { get; set; }

        [MaxStringLength(1000)]
        [Display(Name = "مدارک علمی و تخصصی", Description = "درصورتیکه غیر از مدارک تحصیلی، مدارک علمی و تخصصی دیگری دارید، در این قسمت وارد نمایید"), DataType(DataType.MultilineText)]
        public string ProfessionalCertificates { get; set; }

        [MaxStringLength(300)]
        [Display(Name = "آدرس محل کار"), DataType(DataType.MultilineText)]
        public string WorkAddress { get; set; }

        [MaxStringLength(50)]
        [Display(Name = "شماره تلفن محل کار")]
        public string WorkPhoneNo { get; set; }

        [Display(Name = "ظرفیت پذیرش", Description = "تعداد جلسات فعال بطور همزمان")]
        public byte Capacity { get; set; }

        [Required]
        [Display(Name = "گروه(های) مشاوره"), UIHint("CheckListBox")]
        public IEnumerable<int> CategoriesIds { get; set; }
        public IEnumerable<SelectListItem> CategoryListItems { get; set; }

        public bool UpdatePending { get; set; }
    }
}