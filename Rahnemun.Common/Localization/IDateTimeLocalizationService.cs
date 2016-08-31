using System;
using Edreamer.Framework.Composition;

namespace Rahnemun.Common
{
    [InterfaceExport]
    public interface IDateTimeLocalizationService
    {
        DateTime ConvertToUserTimeZone(DateTime dateTime, int? userId = null);
    }
}