using System;
using System.Linq;
using Rahnemun.CategoryContracts;
using Rahnemun.Domain;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRahnemunDataContext _dataContext;

        public MessageService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<MessageModel> Messages
        {
            get
            {
                return _dataContext.Messages.Select(m => new MessageModel
                {
                    Id = m.Id,
                    Text = m.Text,
                    ByConsultee = m.ByConsultee,
                    SentTime = m.SentTime,
                    SeenTime = m.SeenTime,
                    MediaId = m.MediaId,
                    Session = new SessionModel
                    {
                        Id = m.SessionId,
                        StartTime = m.Session.StartTime,
                        StopTime = m.Session.StopTime,
                        StopType = m.Session.StopType,
                        Rating = m.Session.Rating,
                        Consultee = new UserModel { Id = m.Session.ConsulteeId },
                        Consultant = new UserModel { Id = m.Session.ConsultantId },
                        Category = new CategoryModel { Id = m.Session.CategoryId }
                    }
                });
            }
        }

        public MessageModel GetMessage(int id)
        {
            return Messages.SingleOrDefault(m => m.Id == id);
        }

        public MessageModel SendMessage(int sessionId, bool byConsultee, string text, int? attachmentMediaId)
        {
            var messageEntity = new Message
                                {
                                    SessionId = sessionId,
                                    Text = text,
                                    ByConsultee = byConsultee,
                                    MediaId = attachmentMediaId,
                                    SentTime = DateTime.UtcNow
                                };
            _dataContext.Messages.Add(messageEntity);
            _dataContext.SaveChanges();
            return GetMessage(messageEntity.Id);
        }

        public DateTime SetMessagesAsSeen(int sessionId, bool byConsultee, DateTime sentUntil)
        {
            var notSeenMessages = _dataContext.Messages.WithTracking()
                .Where(m => m.Session.Id == sessionId && m.SeenTime == null &&
                            m.ByConsultee == byConsultee && m.SentTime <= sentUntil);
            var utcNow = DateTime.UtcNow;
            foreach (var message in notSeenMessages)
                message.SeenTime = utcNow;
            _dataContext.SaveChanges();

            return utcNow;
        }
    }
}