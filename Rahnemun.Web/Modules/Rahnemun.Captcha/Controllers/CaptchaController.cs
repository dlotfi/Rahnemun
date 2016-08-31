using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Rahnemun.CaptchaContracts;

namespace Rahnemun.Captcha.Controllers
{
    public class CaptchaController : Controller
    {
        private readonly ICaptchaService _captchaService;

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        // GET: /captcha/{Id}
        [HttpGet]
        public FileContentResult Image(Guid id)
        {
            var bitmap = _captchaService.GetCaptchaImage(id);
            using (var stream = new MemoryStream())
            {
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                var encoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Gif.Guid);
                bitmap.Save(stream, encoder, encoderParameters);
                return File(stream.GetBuffer(), "image/gif");
            }
        }
    }
}
