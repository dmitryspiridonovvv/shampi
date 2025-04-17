using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DrawingPixelFormat = System.Drawing.Imaging.PixelFormat;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace MinerGameLib
{
    public static class Texture
    {
        private static readonly Dictionary<string, int> textures = new Dictionary<string, int>();

        public static int LoadTexture(string path)
        {
            if (textures.TryGetValue(path, out int textureId))
            {
                return textureId;
            }

            // Выводим текущую директорию и полный путь к файлу
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine($"Attempting to load texture from: {Path.GetFullPath(path)}");
            Console.WriteLine($"File exists: {File.Exists(path)}");

            textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            try
            {
                using (var image = new Bitmap(path))
                {
                    Console.WriteLine($"Loading texture {path}: {image.Width}x{image.Height}");
                    var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly,
                        DrawingPixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                        GLPixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    image.UnlockBits(data);
                }
            }
            catch (System.ArgumentException ex)
            {
                Console.WriteLine($"Failed to load texture {path}: {ex.Message}");
                throw;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine($"Texture file not found: {path}. Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading texture {path}: {ex.Message}");
                throw;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            textures[path] = textureId;
            return textureId;
        }

        public static void BindTexture(string path)
        {
            if (textures.TryGetValue(path, out int textureId))
            {
                GL.BindTexture(TextureTarget.Texture2D, textureId);
            }
            else
            {
                Console.WriteLine($"Texture {path} not found!");
            }
        }

        public static void DisposeAll()
        {
            foreach (var texture in textures.Values)
            {
                GL.DeleteTexture(texture);
            }
            textures.Clear();
        }
    }
}