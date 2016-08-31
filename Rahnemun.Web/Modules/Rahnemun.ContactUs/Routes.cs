using Edreamer.Framework.Mvc.Routes;
using Rahnemun.ContactUsContracts;

namespace Rahnemun.ContactUs
{
    public class ContactUsRoute : NamedRoute, IContactUsRoute { }

    public class ContactUsRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<ContactUsRoute>("contact", new { Controller = "ContactUs", Action = "Contact" });
        }
    }
}