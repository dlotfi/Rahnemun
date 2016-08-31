using System;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Media;
using Edreamer.Framework.Media.Image;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.MediaContracts;

namespace Rahnemun.Media.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IImageService _imageService;

        public MediaController(IMediaService mediaService, IImageService imageService)
        {
            _mediaService = mediaService;
            _imageService = imageService;
        }

        // GET: /image/{Id}?Size=...&DefaultImagePath=~/...
        [HttpGet]
        public FileContentResult Image(ImageRouteModel model)
        {
            var image = GetImage(model.Id, model.Size, model.MaxFit, model.DefaultImagePath, this.ResolveLocalUrl("~/Images/default.png"));
            return File(image.Data, image.Type);
        }

        private Edreamer.Framework.Media.Media GetImage(int? id, ImageSize? size, bool maxFit, string alternativeImagePath, string defaultImagePath)
        {
            Edreamer.Framework.Media.Media image = null;

            if (id != null)
            {
                image = _mediaService.GetMedia((int) id);
            }

            if (image == null || image.TypeGroup.ToLower() != "image")
            {
                image = GetDefaultImage(alternativeImagePath, defaultImagePath);
            }

            if (image != null && size != null)
            {
                var sizeInPixels = ((ImageSize)size).InPixels();
                image = _imageService.ManipulateImage(image, new ImageManipulation { Width = sizeInPixels.Width, Height = sizeInPixels.Height, Scale = ScaleMode.Both, FitMode = maxFit ? FitMode.Max : FitMode.Crop });
            }

            return image;
        }

        private Edreamer.Framework.Media.Media GetDefaultImage(string alternativeImagePath, string defaultImagePath)
        {
            Edreamer.Framework.Media.Media image = null;

            if (!String.IsNullOrEmpty(alternativeImagePath))
            {
                image = _mediaService.CreateMedia(EnsureRootRelativePath(alternativeImagePath));
            }

            if ((image == null || !image.IsImage()) && !String.IsNullOrEmpty(defaultImagePath))
            {
                image = _mediaService.CreateMedia(EnsureRootRelativePath(defaultImagePath));
                if (!image.IsImage())
                {
                    image = null;
                }
            }

            return image;
        }

        private static string EnsureRootRelativePath(string path)
        {
            if (!path.StartsWith("~"))
            {
                Throw.IfNot(path.StartsWith("/"))
                    .AnArgumentException("path is not started with /.", "path");
                return "~" + path;
            }
            return path;
        }
    }
}
