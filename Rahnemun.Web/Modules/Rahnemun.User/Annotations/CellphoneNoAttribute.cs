using System.ComponentModel.DataAnnotations;

namespace Rahnemun.User.Annotations
{
    public class CellphoneNoAttribute: RegularExpressionAttribute
    {
        public CellphoneNoAttribute()
            // Regular expression source: http://barnamenevisan.org/Articles/Article1872.html
            : base("09(1[0-9]|3[1-9]|2[1-9])[0-9]{7}")
        {
            ErrorMessage = "The value of the field {0} should be a valid cellphone number.";
        }
    }
}