using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Common
{
    public class CurrencyAttribute : RangeAttribute
    {
        public CurrencyAttribute() : base(typeof(decimal), "0", decimal.MaxValue.ToString())
        {
        }
    }
}
