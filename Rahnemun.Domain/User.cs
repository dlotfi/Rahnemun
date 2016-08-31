using System;
using System.ComponentModel.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.Domain
{
    public class User
    {
        public int Id { get; set; }
        public byte[] Timestamp { get; set; }
        [Required, MaxLength(30)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = true), MaxLength(30)]
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        [MinLength(11), MaxLength(11)]
        public string CellphoneNo { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool SubscribedToNewsletter { get; set; }
        [MaxLength(2000)]
        public string More { get; set; }
        public int? ProfilePictureId { get; set; }
    }
}
