namespace Rahnemun.CategoryContracts
{
    public class CategoryIdModel
    {
        public int CategoryId { get; set; }

        public static implicit operator CategoryIdModel(int value)
        {
            return new CategoryIdModel { CategoryId = value };
        }
    }
}