namespace Rahnemun.Common
{
    public class ReturnUrlModel
    {
        public string ReturnUrl { get; set; }

        public static implicit operator ReturnUrlModel(string value)
        {
            return new ReturnUrlModel { ReturnUrl = value};
        }

        public static implicit operator ReturnUrlModel(System.Uri value)
        {
            return new ReturnUrlModel { ReturnUrl = value.OriginalString };
        }
    }
}
