using System;
using Rahnemun.Common;

namespace Rahnemun.Domain
{
    public class Session
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public SessionStopType? StopType { get; set; }
        public byte? Rating { get; set; }

        public int ConsulteeId { get; set; }
        public User Consultee { get; set; }

        public int ConsultantId { get; set; }
        public User Consultant { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}