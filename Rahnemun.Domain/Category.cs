using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Domain
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Caption { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(8000)]
        public string Terms { get; set; }

        public byte DisplayOrder { get; set; }

        public int CategoryGroupId { get; set; }
        public CategoryGroup CategoryGroup { get; set; }
    }
}
