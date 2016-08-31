using System.Collections.Generic;

namespace Rahnemun.PaymentContracts
{
    public class PaymentRequestPlan
    {
        public PaymentRequestPlan(bool post, string url, IDictionary<string, string> data)
        {
            Post = post;
            Url = url;
            Data = data;
        }

        public bool Post { get; private set; }
        public string Url { get; private set; }
        public IDictionary<string, string> Data { get; private set; }
    }
}