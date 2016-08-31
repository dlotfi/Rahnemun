using System.Linq;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session.Services
{
    public class UserNewMessagesNotificationProvider: IUserNotificationProvider
    {
        private readonly IMessageService _messageService;
        private readonly IConsultantService _consultantService;

        public UserNewMessagesNotificationProvider(IMessageService messageService, IConsultantService consultantService)
        {
            _messageService = messageService;
            _consultantService = consultantService;
        }

        public UserNotificationModel GetUserNotification(int userId)
        {
            var newMessagesCount = _consultantService.IsConsultant(userId)
                ? _messageService.Messages.Count(m => m.Session.Consultant.Id == userId && m.ByConsultee && m.SeenTime == null)
                : _messageService.Messages.Count(m => m.Session.Consultee.Id == userId && !m.ByConsultee && m.SeenTime == null);

            return new UserNotificationModel
                   {
                       Count = newMessagesCount,
                       Title = "پیام های خوانده نشده"
                   };
        }
    }
}