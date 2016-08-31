using Edreamer.Framework.Composition;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IUserNotificationProvider
    {
        UserNotificationModel GetUserNotification(int userId);
    }
}