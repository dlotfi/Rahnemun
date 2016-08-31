using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;

namespace Rahnemun.Domain
{
    public class Guest
    {
        public int Id { get; set; }

        [ShortString, EmailAddress]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(512)]
        public string UserAgent { get; set; }
        [Required, MaxLength(50)]
        public string UserIP { get; set; }
    }
}
