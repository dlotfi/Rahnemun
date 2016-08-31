using System;
using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Domain
{
    public class Message
    {
        public int Id { get; set; }
        [Required, MaxLength(8000)]
        public string Text { get; set; }
        public bool ByConsultee { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime? SeenTime { get; set; }

        public int? MediaId { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}