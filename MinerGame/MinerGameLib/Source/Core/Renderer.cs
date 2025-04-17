using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.IO;

namespace MinerGame.Core
{
    public class Renderer : IDisposable
    {
        private readonly int _shaderProgram;
        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _textureUniform;
        private readonly int _modelUniform;
        private readonly int _projectionUniform;
        public readonly TextureManager TextureManager;
        private Matrix4 _projection;

        public Renderer(Vector2i windowSize)
        {
            TextureManager = new TextureManager();

            // Настройка проекционной матрицы
            _projection = Matrix4.CreateOrthographicOffCenter(0, windowSize.X, windowSize.Y, 0, -1f, 1f);

            string vertexShaderSource = File.ReadAllText("Resources/Shaders/sprite.vert");
            string fragmentShaderSource = File.ReadAllText("Resources/Shaders/sprite.frag");

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception($"Vertex Shader Error: {infoLog}");
            }

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception($"Fragment Shader Error: {infoLog}");
            }

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);
            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_shaderProgram);
                throw new Exception($"Shader Program Error: {infoLog}");
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            float[] vertices =
            {
                 0.0f,  0.0f,  0.0f, 0.0f,
                 1.0f,  0.0f,  1.0f, 0.0f,
                 1.0f,  1.0f,  1.0f, 1.0f,
                 0.0f,  1.0f,  0.0f, 1.0f
            };

            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();

            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.UseProgram(_shaderProgram);
            _textureUniform = GL.GetUniformLocation(_shaderProgram, "sprite");
            _modelUniform = GL.GetUniformLocation(_shaderProgram, "model");
            _projectionUniform = GL.GetUniformLocation(_shaderProgram, "projection");

            // Установка проекционной матрицы
            GL.UniformMatrix4(_projectionUniform, false, ref _projection);
        }

        public void UpdateWindowSize(Vector2i windowSize)
        {
            _projection = Matrix4.CreateOrthographicOffCenter(0, windowSize.X, windowSize.Y, 0, -1f, 1f);
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_projectionUniform, false, ref _projection);
        }

        public int LoadTexture(string path)
        {
            return TextureManager.LoadTexture(path);
        }

        public void UnloadTexture(int textureId)
        {
            TextureManager.UnloadTexture(textureId);
        }

        public void DrawSprite(int textureId, Vector2 position, Vector2 size, Vector4 texCoords)
        {
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);

            Matrix4 model = Matrix4.CreateScale(size.X, size.Y, 1.0f) *
                           Matrix4.CreateTranslation(position.X, position.Y, 0.0f);
            GL.UniformMatrix4(_modelUniform, false, ref model);

            GL.ActiveTexture(TextureUnit.Texture0);
            TextureManager.BindTexture(textureId);
            GL.Uniform1(_textureUniform, 0);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            GL.BindVertexArray(0);

            // Диагностика ошибок OpenGL
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine($"OpenGL Error in DrawSprite: {error}");
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shaderProgram);
            TextureManager.Dispose();
        }
    }
}