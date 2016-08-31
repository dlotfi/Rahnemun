using Rahnemun.Common;
using Rahnemun.UserContracts;

namespace Rahnemun.ContactUsContracts
{
    public class CustomerMessageModel
    {
        public int Id { get; set; }
        public CustomerMessageSubject Subject { get; set; }
        public string Message { get; set; }
        public UserModel User { get; set; }
        public GuestModel Guest { get; set; }
    }
}
