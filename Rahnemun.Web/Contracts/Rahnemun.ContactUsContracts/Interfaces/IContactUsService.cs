using System.Linq;
using Edreamer.Framework.Composition;
using Rahnemun.Common;

namespace Rahnemun.ContactUsContracts
{
    [InterfaceExport]
    public interface IContactUsService
    {
        //IQueryable<CustomerMessageModel> CustomerMessages { get; }
        //CustomerMessageModel GetCustomerMessage(int id);

        void PostCustomerMessage(CustomerMessageSubject subject, string message, int? userId, int? guestId);

    }
}
