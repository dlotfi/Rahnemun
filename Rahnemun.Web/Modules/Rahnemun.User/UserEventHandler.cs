using Edreamer.Framework.Security.Users;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserEventHandler: IUserEventHandler
    {
        private readonly IGuestService _guestService;

        public UserEventHandler(IGuestService guestService)
        {
            _guestService = guestService;
        }

        public void Creating(UserContext context)
        {
        }

        public void Created(UserContext context)
        {
        }

        public void LoggedIn(Edreamer.Framework.Security.User user)
        {
            // Whenever user is logged in, clear guest cookie
            _guestService.ClearCurrentGuest();
        }

        public void LoggedOut(Edreamer.Framework.Security.User user)
        {
        }

        public void AccessDenied(Edreamer.Framework.Security.User user)
        {
        }

        public void ChangedPassword(Edreamer.Framework.Security.User user)
        {
        }
    }
}