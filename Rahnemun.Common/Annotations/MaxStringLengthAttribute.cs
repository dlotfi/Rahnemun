using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Common
{
    public class MaxStringLengthAttribute: StringLengthAttribute
    {
        public MaxStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessage = "The length of field {0} should not exceed {1} characters.";
        }
    }
}