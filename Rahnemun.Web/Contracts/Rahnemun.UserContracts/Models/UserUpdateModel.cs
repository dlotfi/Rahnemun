using System;
using Rahnemun.Common;

namespace Rahnemun.UserContracts
{
    public class UserUpdateModel
    {
        public int Id { get; set; }
        public byte[] Timestamp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string CellphoneNo { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool SubscribedToNewsletter { get; set; }
        public string More { get; set; }
        public int? ProfilePictureId { get; set; }
    }
}
