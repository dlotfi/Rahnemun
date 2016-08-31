using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Common
{
    public class FixedStringLengthAttribute: StringLengthAttribute
    {
        public FixedStringLengthAttribute(int fixedLength)
            : base(fixedLength)
        {
            MinimumLength = fixedLength;
            ErrorMessage = "The length of field {0} must be {1} characters.";
        }
    }
}