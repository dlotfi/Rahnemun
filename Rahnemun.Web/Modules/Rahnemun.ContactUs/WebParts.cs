using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.UIContracts;

namespace Rahnemun.ContactUs
{
    public class FeedbackWebPart : ActionWebPart, IFeedbackWebPart
    {
        public FeedbackWebPart() : base("ContactUs", "FeedbackWebPart") { }
    }
}