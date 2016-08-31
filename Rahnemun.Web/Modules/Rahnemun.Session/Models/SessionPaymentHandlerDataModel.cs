using System;

namespace Rahnemun.Session.Models
{
    [Serializable]
    public class SessionPaymentHandlerDataModel
    {
        public int ConsulteeId { get; set; }
        public int ConsultantId { get; set; }
        public int CategoryId { get; set; }
    }
}