using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGame.Core
{
    public class Shader : IDisposable
    {
        private readonly int _program;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexSource = System.IO.File.ReadAllText(vertexPath);
            string fragmentSource = System.IO.File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
                throw new Exception($"Vertex shader error: {GL.GetShaderInfoLog(vertexShader)}");

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
                throw new Exception($"Fragment shader error: {GL.GetShaderInfoLog(fragmentShader)}");

            _program = GL.CreateProgram();
            GL.AttachShader(_program, vertexShader);
            GL.AttachShader(_program, fragmentShader);
            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
                throw new Exception($"Program link error: {GL.GetProgramInfoLog(_program)}");

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(_program);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_program, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_program);
        }
    }
}