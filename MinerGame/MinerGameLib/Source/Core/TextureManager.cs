using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace MinerGame.Core
{
    public class TextureManager
    {
        private readonly Dictionary<string, int> _textures = new();

        public int LoadTexture(string path)
        {
            if (_textures.TryGetValue(path, out int textureId))
                return textureId;

            textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            using (var stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            _textures[path] = textureId;

            return textureId;
        }

        public void BindTexture(int textureId)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }

        public void UnloadTexture(int textureId)
        {
            if (_textures.ContainsValue(textureId))
            {
                GL.DeleteTexture(textureId);
                _textures.Remove(_textures.First(x => x.Value == textureId).Key);
            }
        }

        public void Dispose()
        {
            foreach (var textureId in _textures.Values)
            {
                GL.DeleteTexture(textureId);
            }
            _textures.Clear();
        }
    }
}