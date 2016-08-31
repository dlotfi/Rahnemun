using System;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.PaymentContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.SessionContracts
{
    public class SessionModel
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public SessionStopType? StopType { get; set; }
        public byte? Rating { get; set; }
        public UserModel Consultee { get; set; }
        public UserModel Consultant { get; set; }
        public CategoryModel Category { get; set; }
        //public PaymentModel Payment { get; set; }
    }
}
