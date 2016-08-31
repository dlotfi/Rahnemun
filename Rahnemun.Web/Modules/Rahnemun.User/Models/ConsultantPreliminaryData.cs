using System;

namespace Rahnemun.User.Models
{
    [Serializable]
    internal class ConsultantPreliminaryData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool SubscribedToNewsletter { get; set; }
    }
}