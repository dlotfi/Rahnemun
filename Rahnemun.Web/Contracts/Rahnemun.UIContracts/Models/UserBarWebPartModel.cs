namespace Rahnemun.UIContracts
{
    public class UserBarWebPartModel
    {
        public bool ResponsiveAlternative { get; set; }

        public static implicit operator UserBarWebPartModel(bool value)
        {
            return new UserBarWebPartModel { ResponsiveAlternative = value };
        }
    }
}