using System;
using System.Collections.Generic;
using System.Drawing;
using Edreamer.Framework.Composition;

namespace Rahnemun.CaptchaContracts
{
    [InterfaceExport]
    public interface ICaptchaService
    {
        Color FontColor { get; set; }

        Color Background { get; set; }

        ushort Width { get; set; }

        ushort Height { get; set; }

        IList<FontFamily> Fonts { get; }

        string Chars { get; set; }

        int Count { get; set; }

        Bitmap GetCaptchaImage(Guid id);

        bool ValidateCaptcha(Guid id, string value);
    }
}