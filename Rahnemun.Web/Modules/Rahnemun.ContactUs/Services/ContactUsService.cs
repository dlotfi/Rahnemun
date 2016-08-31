using System;
using Edreamer.Framework.Helpers;
using Rahnemun.Common;
using Rahnemun.ContactUsContracts;
using Rahnemun.Domain;

namespace Rahnemun.ContactUs.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IRahnemunDataContext _dataContext;

        public ContactUsService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public void PostCustomerMessage(CustomerMessageSubject subject, string message, int? userId, int? guestId)
        {
            Throw.If(userId == null && guestId == null || userId != null && guestId != null)
                .AnArgumentException("One of two parameters userId or guestId must be specified.");

            var customerMessageEntity = new CustomerMessage
            {
                Subject = subject,
                Message = message,
                UserId = userId,
                GuestId = guestId,
                SentTime = DateTime.UtcNow
            };
            _dataContext.CustomerMessages.Add(customerMessageEntity);
            _dataContext.SaveChanges();
        }
    }
}