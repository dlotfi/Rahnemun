using Rahnemun.MediaContracts;
using Edreamer.Framework.Mvc.WebParts;

namespace Rahnemun.Media
{
    public class ImageWebPart : SimpleWebPart<ImageWebPartModel>, IImageWebPart
    {
        public ImageWebPart() : base("ImageWebPart") { }
    }
}