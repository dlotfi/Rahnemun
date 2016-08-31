using Rahnemun.Common;

namespace Rahnemun.Session.Models
{
    public class ActiveSessionDashboardViewModel
    {
        public string SessionUrl { get; set; }
        public string CategoryCaption { get; set; }
        public string FullName { get; set; }
        public int? ProfilePictureId { get; set; }
        public Gender? Gender { get; set; }
        public string ProfileUrl { get; set; }
        public int UnreadMessagesAmount { get; set; }
        
    }
}