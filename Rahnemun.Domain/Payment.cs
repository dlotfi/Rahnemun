using System;
using System.ComponentModel.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.Domain
{
    public class Payment
    {
        public int Id { get; set; }

        [Currency]
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        public string HandlerId { get; set; }

        [Required, MaxLength(8000)]
        public string HandlerData { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Time { get; set; }

        [MaxLength(50)]
        public string ProviderName { get; set; }

        [MaxLength(50)]
        public string RequestId { get; set; }
        public DateTime? RequestTime { get; set; }

        [MaxLength(50)]
        public string ReferenceId { get; set; }

        [MaxLength(5)]
        public string RequestResult { get; set; }

        public DateTime? VerificationTime { get; set; }

        [MaxLength(5)]
        public string VerificationResult { get; set; }
    }
}