using System;
using System.Globalization;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Composition;

namespace Rahnemun.Common
{
    // Based on an answer in: http://stackoverflow.com/questions/10293440/how-to-make-asp-net-mvc-model-binder-treat-incomming-date-as-utc
    // This model binder perserves the DateTimeKind of the DateTime, unlike default model binder which always generates local DateTimes.
    [Binder(typeof(DateTime), typeof(DateTime?))]
    public class DateTimeBinder: IModelBinder 
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var name = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(name);
            if (String.IsNullOrEmpty(value?.AttemptedValue)) return null;
            if (value.RawValue is DateTime) return value.RawValue;

            try
            {
                return DateTime.Parse(value.AttemptedValue, null, DateTimeStyles.RoundtripKind);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(name, ex);
                return null;
            }
        }
    }
}
