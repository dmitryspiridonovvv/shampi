using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Menu : IDisposable
    {
        private readonly int width, height;
        private readonly string[] menuOptions = { "Start", "Settings", "Exit" };
        public int SelectedOption { get; private set; }
        private readonly List<int> textTextures;
        private readonly List<int> vaos, vbos;

        public Menu(int width, int height)
        {
            this.width = width;
            this.height = height;
            SelectedOption = 0;
            textTextures = new List<int>();
            vaos = new List<int>();
            vbos = new List<int>();
        }

        public void Initialize()
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                int textureId = CreateTextTexture(menuOptions[i], i == SelectedOption ? Color.Yellow : Color.White);
                textTextures.Add(textureId);

                int vao = GL.GenVertexArray();
                int vbo = GL.GenBuffer();
                vaos.Add(vao);
                vbos.Add(vbo);

                float textWidth = 200;
                float textHeight = 50;

                // Вершины начинаются с (0, 0), позиция будет задаваться через modelMatrix
                float[] vertices = new float[]
                {
                    0,         0,          0, 1,
                    textWidth, 0,          1, 1,
                    textWidth, textHeight, 1, 0,

                    0,         0,          0, 1,
                    textWidth, textHeight, 1, 0,
                    0,         textHeight, 0, 0
                };

                GL.BindVertexArray(vao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                GL.BindVertexArray(0);
            }
        }

        private int CreateTextTexture(string text, Color color)
        {
            using (var bitmap = new Bitmap(200, 50))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent);
                using (var font = new Font("Arial", 16))
                using (var brush = new SolidBrush(color))
                {
                    graphics.DrawString(text, font, brush, new PointF(0, 0));
                }

                int textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);

                var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                Console.WriteLine($"Created texture for '{text}' with ID: {textureId}");
                return textureId;
            }
        }

        public void MoveUp()
        {
            SelectedOption = Math.Max(0, SelectedOption - 1);
            UpdateTextures();
        }

        public void MoveDown()
        {
            SelectedOption = Math.Min(menuOptions.Length - 1, SelectedOption + 1);
            UpdateTextures();
        }

        private void UpdateTextures()
        {
            for (int i = 0; i < textTextures.Count; i++)
            {
                Console.WriteLine($"Deleting texture ID: {textTextures[i]} for '{menuOptions[i]}'");
                GL.DeleteTexture(textTextures[i]);
                textTextures[i] = CreateTextTexture(menuOptions[i], i == SelectedOption ? Color.Yellow : Color.White);
            }
        }

        public void Render(Shader shader)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"Rendering menu option: {menuOptions[i]} at VAO {vaos[i]}");
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, textTextures[i]);

                float textWidth = 200;
                float textHeight = 50;
                float x = (width - textWidth) / 2;
                float y = (height - (menuOptions.Length * textHeight)) / 2 + (menuOptions.Length - 1 - i) * 60;
                Matrix4 modelMatrix = Matrix4.CreateTranslation(x, y, 0);
                shader.SetMatrix4("uModel", modelMatrix);

                GL.BindVertexArray(vaos[i]);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                GL.BindVertexArray(0);

                var error = GL.GetError();
                if (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
                {
                    Console.WriteLine($"OpenGL Error in Menu.Render (Option {i}): {error}");
                }
            }
        }

        public void RenderSettings(Shader shader)
        {
            int textureId = CreateTextTexture("Settings - Press ESC to return", Color.White);
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();

            float x = (width - 300) / 2;
            float y = height / 2;
            float textWidth = 300;
            float textHeight = 50;

            float[] vertices = new float[]
            {
                0,         0,          0, 1,
                textWidth, 0,          1, 1,
                textWidth, textHeight, 1, 0,

                0,         0,          0, 1,
                textWidth, textHeight, 1, 0,
                0,         textHeight, 0, 0
            };

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BindTexture(TextureTarget.Texture2D, textureId);

            Matrix4 modelMatrix = Matrix4.CreateTranslation(x, y, 0);
            shader.SetMatrix4("uModel", modelMatrix);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.DeleteTexture(textureId);
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }

        public void Dispose()
        {
            foreach (var texture in textTextures)
                GL.DeleteTexture(texture);
            foreach (var vbo in vbos)
                GL.DeleteBuffer(vbo);
            foreach (var vao in vaos)
                GL.DeleteVertexArray(vao);
        }
    }
}