using System;

namespace Rahnemun.SessionContracts
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool ByConsultee { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime? SeenTime { get; set; }
        public int? MediaId { get; set; }
        public SessionModel Session { get; set; }
    }
}
