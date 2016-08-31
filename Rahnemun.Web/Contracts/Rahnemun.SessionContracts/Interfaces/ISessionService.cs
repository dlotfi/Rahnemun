using System;
using System.Linq;
using Edreamer.Framework.Composition;
using Rahnemun.Common;

namespace Rahnemun.SessionContracts
{
    [InterfaceExport]
    public interface ISessionService
    {
        IQueryable<SessionModel> Sessions { get; }
        SessionModel GetSession(int id);
        SessionModel CreateSession(int consulteeId, int consultantId, int categoryId, int paymentId);
        DateTime StopSession(int id, SessionStopType sessionStopType);
        void DeleteSession(int id);
        void RateSession(int id, byte ratingValue);

        int GetSessionElapsedTime(int id);
    }
}
