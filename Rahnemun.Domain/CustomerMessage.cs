using System;
using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.Domain
{
    public class CustomerMessage
    {
        public int Id { get; set; }
        
        public CustomerMessageSubject Subject { get; set; }
        [Required, MaxLength(2000)]
        public string Message { get; set; }
        public DateTime SentTime { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
