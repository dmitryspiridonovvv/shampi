using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public abstract class GameObject : IDisposable
    {
        public Vector2 Position { get; set; }
        public bool IsActive { get; set; } = true;
        protected int textureId;
        protected float width = 32, height = 32;
        private int vao, vbo;
        private readonly float[] vertices;

        public GameObject(Vector2 position, string textureName)
        {
            Position = position;
            textureId = Texture.LoadTexture(textureName);

            vertices = new float[]
            {
                // Переворачиваем текстурные координаты по оси v
                0,     0,      0, 1, // Левый нижний угол (было 0, 0)
                width, 0,      1, 1, // Правый нижний угол (было 1, 0)
                width, height, 1, 0, // Правый верхний угол (было 1, 1)

                0,     0,      0, 1, // Левый нижний угол (было 0, 0)
                width, height, 1, 0, // Правый верхний угол (было 1, 1)
                0,     height, 0, 0  // Левый верхний угол (было 0, 1)
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

        public virtual void Update(float deltaTime, List<GameObject> gameObjects) { }

        public virtual void Render(Shader shader)
        {
            if (!IsActive) return;

            GL.Enable(EnableCap.Texture2D);
            Texture.BindTexture(GetTextureName());

            Matrix4 modelMatrix = Matrix4.CreateTranslation(Position.X, Position.Y, 0);
            shader.SetMatrix4("uModel", modelMatrix);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            var error = GL.GetError();
            if (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error in GameObject.Render ({GetType().Name}): {error}");
            }
        }

        protected abstract string GetTextureName();

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}