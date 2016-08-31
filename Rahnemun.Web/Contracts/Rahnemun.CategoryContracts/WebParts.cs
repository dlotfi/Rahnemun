using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.UIContracts;

namespace Rahnemun.CategoryContracts
{
    public interface ICategoryListWebPart : IWebPart { }
    public interface ICategoryMenuWebPart : IWebPart<CategoryMenuWebPartModel> { }
}
