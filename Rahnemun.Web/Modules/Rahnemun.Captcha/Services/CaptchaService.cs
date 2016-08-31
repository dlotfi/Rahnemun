using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Edreamer.Framework.Caching;
using Edreamer.Framework.Caching.VolatileProviders;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Rahnemun.CaptchaContracts;

namespace Rahnemun.Captcha.Services
{
    public class CaptchaService: ICaptchaService
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IClock _clock;
        private const float V = 5F;

        public CaptchaService(IWorkContextAccessor workContextAccessor, IClock clock)
        {
            _workContextAccessor = workContextAccessor;
            _clock = clock;

            Width = 120;
            Height = 30;
            Fonts = new List<FontFamily>
                        {
                            new FontFamily("Times New Roman"),
                            new FontFamily("Georgia"),
                            new FontFamily("Arial"),
                            new FontFamily("Comic Sans MS")
                        };
            FontColor = Color.FromArgb(0x39, 0x89, 0xc9); //Color.Blue;
            Background = Color.White;
            Chars = "QWERTYUIPASDFGHJKLZCVBNM";
            Count = 4;
            Timeout = 300;
        }

        public Color FontColor { get; set; }
        public Color Background { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public IList<FontFamily> Fonts { get; private set; }
        public string Chars { get; set; }
        public int Count { get; set; }
        public int Timeout { get; set; }

        private IDictionary<Guid, CaptchaEntry> ValidCaptchaEntries
        {
            get
            {
                var httpContext = _workContextAccessor.Context.CurrentHttpContext();
                if (httpContext.Session["Captcha"] == null)
                    httpContext.Session["Captcha"] = new Dictionary<Guid, CaptchaEntry>();
                var captchaEntries = (Dictionary<Guid, CaptchaEntry>) httpContext.Session["Captcha"];
                captchaEntries.RemoveRange(captchaEntries.Where(c => !c.Value.Token.IsCurrent)); //Cleanup
                return captchaEntries;
            }
        }

        public Bitmap GetCaptchaImage(Guid id)
        {
            var captchaEntries = ValidCaptchaEntries;
            var text = GenerateText(Chars, Count);
            var image = GenerateImage(text, Width, Height);
            var token = _clock.When(new TimeSpan(0, 0, Timeout));
            captchaEntries[id] = new CaptchaEntry { Image = image, Value = text, Token = token };
            return captchaEntries[id].Image;
        }

        public bool ValidateCaptcha(Guid id, string value)
        {
            var captchaEntries = ValidCaptchaEntries;
            return captchaEntries.ContainsKey(id) && captchaEntries[id].Value.EqualsIgnoreCase(value);
        }

        private Bitmap GenerateImage(string text, ushort width, ushort height)
        {
            var random = new Random();
            //Randomly choose the font name.
            var familyName = Fonts[random.Next(Fonts.Count - 1)];
            // Create a new 32-bit bitmap image.
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, width, height);

                // Fill in the background.
                using (var brush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Background))
                {
                    g.FillRectangle(brush, rect);

                    // Set up the text format.
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                        Trimming = StringTrimming.None
                    };

                    format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, text.Length) });

                    // Set up the text font.
                    RectangleF size;
                    float fontSize = rect.Height + 1;
                    Font font = null;
                    // Adjust the font size until the text fits within the image.
                    do
                    {
                        if (font != null)
                            font.Dispose();
                        fontSize--;
                        font = new Font(familyName, fontSize, FontStyle.Bold);
                        size = g.MeasureCharacterRanges(text, font, rect, format)[0].GetBounds(g);
                    } while (size.Width > rect.Width || size.Height > rect.Height);
                    // Check http://stackoverflow.com/questions/2292812/font-in-graphicspath-addstring-is-smaller-than-usual-font on why we have to convert to em
                    // Create a path using the text and warp it randomly.
                    var path = new GraphicsPath();
                    path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, rect, format);

                    PointF[] points =
                        {
                            new PointF(random.Next(rect.Width)/V, random.Next(rect.Height)/V), 
                            new PointF(rect.Width - random.Next(rect.Width)/V, random.Next(rect.Height)/V),
                            new PointF(random.Next(rect.Width)/V, rect.Height - random.Next(rect.Height)/V),
                            new PointF(rect.Width - random.Next(rect.Width)/V, rect.Height - random.Next(rect.Height)/V)
                        };
                    var matrix = new Matrix();
                    matrix.Translate(0F, 0F);
                    path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

                    // Draw the text.
                    using (var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, FontColor))
                    {
                        g.FillPath(hatchBrush, path);

                        // Add some random noise.
                        var m = Math.Max(rect.Width, rect.Height);
                        for (var i = 0; i < (int)(rect.Width * rect.Height / 40F); i++)
                        {
                            var x = random.Next(rect.Width);
                            var y = random.Next(rect.Height);
                            var w = random.Next(m / 30);
                            var h = random.Next(m / 30);
                            g.FillEllipse(hatchBrush, x, y, w, h);
                        }
                    }
                    // Clean up.
                    font.Dispose();
                }

                return bitmap;
            }
        }

        private static string GenerateText(string chars, int count)
        {
            var lenght = RandomNumber(0) + count;
            var output = new StringBuilder(lenght);

            for (var i = 0; i < lenght; i++)
            {
                var randomIndex = RandomNumber(chars.Length - 1);
                output.Append(chars[randomIndex]);
            }

            return output.ToString();
        }

        private static readonly byte[] Randb = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
        private static int RandomNumber(int max)
        {
            Rand.GetBytes(Randb);
            var value = BitConverter.ToInt32(Randb, 0);
            return Math.Abs(value) % (max + 1);
        }

        private class CaptchaEntry
        {
            public Bitmap Image { get; set; }
            public string Value { get; set; }
            public IVolatileToken Token { get; set; }
        }
    }
}