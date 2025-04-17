using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Background : IDisposable
    {
        private int textureId;
        private int vao, vbo;
        private readonly float[] vertices;
        private readonly int width, height;

        public Background(int width, int height)
        {
            this.width = width;
            this.height = height;

            textureId = Texture.LoadTexture("background.png");

            vertices = new float[]
            {
                0,     0,      0, 0,
                width, 0,      1, 0,
                width, height, 1, 1,

                0,     0,      0, 0,
                width, height, 1, 1,
                0,     height, 0, 1
            };

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }

        public void Render(Shader shader)
        {
            GL.Enable(EnableCap.Texture2D);
            Texture.BindTexture("background.png");

            Matrix4 modelMatrix = Matrix4.CreateTranslation(0, 0, 0);
            shader.SetMatrix4("uModel", modelMatrix);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            var error = GL.GetError();
            if (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error in Background.Render: {error}");
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}