using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;
using MinerGame.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace MinerGameLib.Source.States
{
    [SupportedOSPlatform("windows")]
    public class RestartPromptState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;
        private readonly Renderer _renderer;

        public RestartPromptState(GameManager gameManager, MenuRenderer menuRenderer, Renderer renderer)
        {
            _gameManager = gameManager;
            _menuRenderer = menuRenderer;
            _renderer = renderer;
        }

        public void Update()
        {
        }

        public void Render()
        {
            var bitmap = _menuRenderer.RenderMenu("Restart?");
            int textureId = ConvertBitmapToTexture(bitmap);
            _renderer.DrawSprite(textureId, new Vector2(0, 0), new Vector2(bitmap.Width, bitmap.Height), new Vector4(0, 0, 1, 1));
            GL.DeleteTexture(textureId);
        }

        public void HandleInput(string input)
        {
            if (input == "Confirm")
            {
                _gameManager.TransitionTo("Confirm");
            }
        }

        private int ConvertBitmapToTexture(Bitmap bitmap)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return textureId;
        }
    }
}