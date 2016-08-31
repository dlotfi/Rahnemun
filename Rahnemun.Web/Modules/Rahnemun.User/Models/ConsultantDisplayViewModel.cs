using System.ComponentModel.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.User.Models
{
    public class ConsultantDisplayViewModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }

        public int? ProfilePictureId { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CategoryCaption { get; set; }
        
        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }

        [Display(Name = "سطح تحصیلات")]
        public EducationLevel EducationLevel { get; set; }

        [Display(Name = "وضعیت تاهل")]
        public MaritalStatus? MaritalStatus { get; set; }

        [Display(Name = "سن")]
        public string Age { get; set; }

        [Display(Name = "توضیحات بیشتر"), DataType(DataType.MultilineText)]
        public string More { get; set; }

        
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "مدارک تحصیلی"), DataType(DataType.MultilineText)]
        public string Education { get; set; }

        [Display(Name = "سابقه کار"), DataType(DataType.MultilineText)]
        public string ProfessionalExperience { get; set; }

        [Display(Name = "مدارک علمی و تخصصی"), DataType(DataType.MultilineText)]
        public string ProfessionalCertificates { get; set; }

        [Display(Name = "آدرس محل کار"), DataType(DataType.MultilineText)]
        public string WorkAddress { get; set; }

        [Display(Name = "شماره تلفن دفتر")]
        public string WorkPhoneNo { get; set; }
       
    }
}