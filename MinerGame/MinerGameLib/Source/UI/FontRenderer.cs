#pragma warning disable CA1416
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace MinerGameLib.Source.UI
{
    public class FontRenderer
    {
        private readonly PrivateFontCollection _fontCollection;
        private Font _font;

        public FontRenderer(string fontPath, float fontSize)
        {
            _fontCollection = new PrivateFontCollection();
            try
            {
                _fontCollection.AddFontFile(fontPath);
                _font = new Font(_fontCollection.Families[0], fontSize, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки шрифта: {ex.Message}");
                _font = new Font("Arial", fontSize);
            }
        }

        public Bitmap RenderText(string text, Color textColor, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                using (var format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    using (var brush = new SolidBrush(textColor))
                    {
                        graphics.DrawString(text, _font, brush, new RectangleF(0, 0, width, height), format);
                    }
                }
            }
            return bitmap;
        }

        public unsafe byte[] GetBitmapData(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            var length = bitmapData.Stride * bitmapData.Height;
            var data = new byte[length];

            fixed (byte* ptr = data)
            {
                Buffer.MemoryCopy((void*)bitmapData.Scan0, ptr, length, length);
            }

            bitmap.UnlockBits(bitmapData);
            return data;
        }

        public void Dispose()
        {
            _font?.Dispose();
            _fontCollection?.Dispose();
        }
    }
}
#pragma warning restore CA1416