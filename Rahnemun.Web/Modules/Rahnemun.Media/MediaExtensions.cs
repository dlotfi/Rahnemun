using System;
using System.Drawing;
using System.IO;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Media;
using Rahnemun.MediaContracts;

namespace Rahnemun.Media
{
    public static class MediaExtensions
    {
        public static Edreamer.Framework.Media.Media CreateMedia(this IMediaService mediaService, string mediaPath)
        {
            var filePath = PathHelpers.GetPhysicalPath(mediaPath);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return mediaService.CreateMedia(fileStream, Path.GetFileName(filePath));
        }

        public static bool IsImage(this Edreamer.Framework.Media.Media media)
        {
            return (media.TypeGroup.ToLower() == "image");
        }

        public static Size InPixels(this ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Comment: return new Size(75, 75);
                case ImageSize.List: return new Size(260, 260);
                case ImageSize.Thumbnail: return new Size(280, 280);
                case ImageSize.Grid: return new Size(360, 360);
                case ImageSize.GridWide: return new Size(360, 240);
                case ImageSize.Content: return new Size(500, 500);
                case ImageSize.Cover: return new Size(730, 480);
                case ImageSize.CoverWide: return new Size(1110, 360);
                default:
                    throw new ArgumentOutOfRangeException("size", size, "Specified ImageSize is not valid.");
            }
        }
    }
}