using System.Collections.Specialized;
using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IGuestService
    {
        IQueryable<GuestModel> Guests { get; }

        GuestModel SetCurrentGuest(string email, string name, NameValueCollection requestData);
        GuestModel GetCurrentGuest();
        void ClearCurrentGuest();
    }
}