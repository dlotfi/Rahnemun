using Edreamer.Framework.Composition;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Services
{
    [PartPriority(PartPriorityAttribute.Minimum)]
    public class NullUserNotificationProvider: IUserNotificationProvider
    {
        public UserNotificationModel GetUserNotification(int userId)
        {
            return new UserNotificationModel
                   {
                       Count = 0,
                       Title = ""
                   };
        }
    }
}