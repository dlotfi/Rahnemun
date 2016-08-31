using Rahnemun.UserContracts;

namespace Rahnemun.User.Models
{
    public class UserBarWebPartViewModel
    {
        public bool ResponsiveAlternative { get; set; }
        public bool UserLoggedIn { get; set; }
        public string Name { get; set; }
        public UserNotificationModel Notification { get; set; }
    }
}