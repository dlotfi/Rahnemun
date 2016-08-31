namespace Rahnemun.CategoryContracts
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Terms { get; set; }
        public byte DisplayOrder { get; set; }
        public CategoryGroupModel CategoryGroup{ get; set; }
    }
}