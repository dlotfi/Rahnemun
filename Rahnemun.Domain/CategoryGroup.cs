using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Domain
{
    public class CategoryGroup
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Caption { get; set; }

        [Required]
        public byte DisplayOrder { get; set; }
    }
}
