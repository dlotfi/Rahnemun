using System.ComponentModel.DataAnnotations;
using System.Web;
using Rahnemun.Common;

namespace Rahnemun.Session.Models
{
    public class MessageViewModel
    {
        [Required]
        [Display(Name = "پیام"), DataType(DataType.MultilineText)]
        public string Text { get; set; }


        [AcceptExtensions(Extensions = "png,jpg,jpeg,gif,pdf")]
        [Display(Name = "پیوست", Description = "فایل با پسوند gif ،png، jpeg، jpg یا pdf و حداکثر حجم 10 مگابایت"), DataType("Upload")]
        public HttpPostedFileBase Attachment { get; set; }
    }
}