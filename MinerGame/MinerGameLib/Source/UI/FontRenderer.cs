using MinerGame.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MinerGame.UI
{
    public class FontRenderer : IDisposable
    {
        private readonly Renderer _renderer;
        private readonly Dictionary<char, Glyph> _glyphCache;
        private readonly Font _font;

        private struct Glyph
        {
            public int TextureId;
            public Vector2 Size;
            public Vector2 Bearing;
            public float Advance;
        }

        public FontRenderer(Renderer renderer, string fontPath, int fontSize = 16)
        {
            _renderer = renderer;
            _glyphCache = new Dictionary<char, Glyph>();

            try
            {
                if (!File.Exists(fontPath))
                    throw new FileNotFoundException($"Font file not found: {fontPath}");

                var fontCollection = new System.Drawing.Text.PrivateFontCollection();
                fontCollection.AddFontFile(fontPath);
                _font = new Font(fontCollection.Families[0], fontSize);
                Console.WriteLine($"Font loaded: {fontPath}, size: {fontSize}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load font: {ex.Message}");
                throw;
            }

            LoadGlyphs(" !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~");
        }

        private void LoadGlyphs(string characters)
        {
            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            var stringFormat = StringFormat.GenericTypographic;

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            foreach (char c in characters)
            {
                if (_glyphCache.ContainsKey(c)) continue;

                var size = graphics.MeasureString(c.ToString(), _font, PointF.Empty, stringFormat);
                int width = (int)Math.Ceiling(size.Width);
                int height = (int)Math.Ceiling(size.Height);

                if (width == 0 || height == 0)
                {
                    Console.WriteLine($"Glyph '{c}' has zero size, skipping");
                    continue;
                }

                using var charBitmap = new Bitmap(width, height);
                using var charGraphics = Graphics.FromImage(charBitmap);
                charGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                charGraphics.DrawString(c.ToString(), _font, Brushes.White, 0, 0, stringFormat);

                var bitmapData = charBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                int texId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                charBitmap.UnlockBits(bitmapData);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

                _glyphCache[c] = new Glyph
                {
                    TextureId = texId,
                    Size = new Vector2(width, height),
                    Bearing = new Vector2(0, height),
                    Advance = width
                };
            }

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);
        }

        public void RenderText(string text, Vector2 position, float scale = 1.0f)
        {
            Console.WriteLine($"Rendering text: '{text}' at {position}");
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            float x = position.X;
            foreach (char c in text)
            {
                if (!_glyphCache.ContainsKey(c)) continue;

                var glyph = _glyphCache[c];
                float xpos = x + glyph.Bearing.X * scale;
                float ypos = position.Y - (glyph.Size.Y - glyph.Bearing.Y) * scale;

                float w = glyph.Size.X * scale;
                float h = glyph.Size.Y * scale;

                if (w > 0 && h > 0)
                {
                    var texCoords = new Vector4(0, 0, 1, 1);
                    _renderer.DrawSprite(glyph.TextureId, new Vector2(xpos, ypos), new Vector2(w, h), texCoords);
                }

                x += glyph.Advance * scale;
            }

            GL.Disable(EnableCap.Blend);
        }

        public void Dispose()
        {
            foreach (var glyph in _glyphCache.Values)
            {
                GL.DeleteTexture(glyph.TextureId);
            }
            _glyphCache.Clear();
            _font?.Dispose();
        }
    }
}