namespace Rahnemun.EmailContracts
{
    public class NewsletterSubscriptionWebPartModel
    {
        public string PageTitle { get; set; }

        public static implicit operator NewsletterSubscriptionWebPartModel(string value)
        {
            return new NewsletterSubscriptionWebPartModel { PageTitle = value };
        }
    }
}