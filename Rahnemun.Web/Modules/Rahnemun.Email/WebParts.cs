using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.EmailContracts;

namespace Rahnemun.Email
{
    public class NewsletterSubscriptionWebPart : SimpleWebPart<NewsletterSubscriptionWebPartModel>, INewsletterSubscriptionWebPart
    {
        public NewsletterSubscriptionWebPart() : base("NewsletterSubscriptionWebPart") { }
    }
}