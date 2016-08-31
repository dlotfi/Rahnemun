using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Edreamer.Framework.Context;
using Edreamer.Framework.Injection;
using Rahnemun.Common;
using Rahnemun.Domain;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Services
{
    public class GuestService: IGuestService
    {
        private readonly IRahnemunDataContext _dataContext;
        private readonly IWorkContextAccessor _workContextAccessor;
        private const string CookieName = ".RAHNEMUNGUEST";

        public GuestService(IRahnemunDataContext dataContext, IWorkContextAccessor workContextAccessor)
        {
            _dataContext = dataContext;
            _workContextAccessor = workContextAccessor;
        }

        public IQueryable<GuestModel> Guests
        {
            get
            {
                return _dataContext.Guests.Select(g => new GuestModel
                {
                    Id = g.Id,
                    Email = g.Email,
                    Name = g.Name,
                    UserAgent = g.UserAgent,
                    UserIP = g.UserIP
                });
            }
        }

        public GuestModel SetCurrentGuest(string email, string name, NameValueCollection requestData)
        {
            var guest = GetCurrentGuest();
            var userAgent = RequestInfoHelper.GetUserAgent(requestData);
            var userIP = RequestInfoHelper.GetUserIP(requestData);
            if (guest == null || 
               (!String.IsNullOrEmpty(email) && guest.Email != email) || 
               (!String.IsNullOrEmpty(name) && guest.Name != name))
            {
                guest = new GuestModel
                {
                    Email = email,
                    Name = name,
                    UserAgent = userAgent,
                    UserIP = userIP
                };
                var guestEntity = Injector.PlaneInject(new Guest(), guest);
                _dataContext.Guests.Add(guestEntity);
                _dataContext.SaveChanges();
                guest.Id = guestEntity.Id;
            }
            var cookie = new HttpCookie(CookieName, guest.Id.ToString())
                         {
                             Expires = DateTime.UtcNow.AddDays(30)
                         };
            _workContextAccessor.Context.CurrentHttpContext().Response.Cookies.Add(cookie);
            return guest;
        }

        public GuestModel GetCurrentGuest()
        {
            var cookie = _workContextAccessor.Context.CurrentHttpContext().Request.Cookies[CookieName];
            int guestId;
            if (cookie == null || !Int32.TryParse(cookie.Value, out guestId)) return null;
            return Guests.SingleOrDefault(g => g.Id == guestId);
        }

        public void ClearCurrentGuest()
        {
            var expiredCookie = new HttpCookie(CookieName, "")
                                {
                                    Expires = DateTime.UtcNow.AddDays(-1)
                                };
            _workContextAccessor.Context.CurrentHttpContext().Response.Cookies.Add(expiredCookie);
        }
    }
}