using System;
using Rahnemun.UserContracts;

namespace Rahnemun.PaymentContracts
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string HandlerId { get; set; }
        public string HandlerData { get; set; }
        public DateTime Time { get; set; }
        public string ProviderName { get; set; }
        public DateTime? RequestTime { get; set; }
        public string RequestId { get; set; }
        public string ReferenceId { get; set; }
        public string RequestResult { get; set; }
        public DateTime? VerificationTime { get; set; }
        public string VerificationResult { get; set; }
        public UserModel User { get; set; }
    }
}