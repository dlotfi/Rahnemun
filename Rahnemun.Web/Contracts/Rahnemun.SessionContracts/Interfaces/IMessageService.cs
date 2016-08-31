using System;
using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.SessionContracts
{
    [InterfaceExport]
    public interface IMessageService
    {
        IQueryable<MessageModel> Messages { get; }
        MessageModel GetMessage(int id);
        MessageModel SendMessage(int sessionId, bool byConsultee, string text, int? attachmentMediaId);
        DateTime SetMessagesAsSeen(int sessionId, bool byConsultee, DateTime sentUntil);
    }
}
