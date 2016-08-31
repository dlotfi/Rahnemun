namespace Rahnemun.UIContracts
{
    public class CategoryMenuWebPartModel
    {
        public bool Active { get; set; }

        public static implicit operator CategoryMenuWebPartModel(bool value)
        {
            return new CategoryMenuWebPartModel { Active = value };
        }
    }
}