using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Rahnemun.Common;
using Rahnemun.User.Annotations;

namespace Rahnemun.User.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public byte[] Timestamp { get; set; }

        [Required, MaxStringLength(30)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required, MaxStringLength(30)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [AcceptExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "تصویر پروفایل", Description = "فایل با پسوند png، jpeg، jpg یا gif و حداکثر حجم 300 کیلوبایت"), DataType("Upload")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        public int? ProfilePictureId { get; set; }

        // ToDo [26071521]: Gender should be nullable but in derived class ConsultantEditViewModwl it is required. Find a way to solve this.
        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }

        [Display(Name = "سطح تحصیلات")]
        public EducationLevel EducationLevel { get; set; }

        [Display(Name = "وضعیت تاهل")]
        public MaritalStatus? MaritalStatus { get; set; }

        [Required, CellphoneNo]
        [Display(Name = "تلفن همراه", Description = "مثال: 09127654321")]
        public string CellphoneNo { get; set; }

        [Display(Name = "تاریخ تولد", Description = "مثال: 1360/01/01")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "مایلم خبرنامۀ رهنمون را دریافت نمایم.", Description = "در صورت عدم تمایل به دریافت خبرنامه، گزینه را غیر فعال نمایید.")]
        public bool SubscribedToNewsletter { get; set; }

        [Display(Name = "توضیحات", Description = "توضیحات بیشتر"), DataType(DataType.MultilineText)]
        public string More { get; set; }
    }
}