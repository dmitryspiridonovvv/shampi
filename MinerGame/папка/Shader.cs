using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Shader : IDisposable
    {
        private readonly int _program;

        public Shader(string vertexPath, string fragmentPath)
        {
            try
            {
                string vertexCode = File.ReadAllText(Path.Combine("Resources", vertexPath));
                string fragmentCode = File.ReadAllText(Path.Combine("Resources", fragmentPath));

                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexCode);
                GL.CompileShader(vertexShader);
                GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vSuccess);
                if (vSuccess == 0)
                {
                    string error = GL.GetShaderInfoLog(vertexShader);
                    Console.WriteLine($"Vertex shader compilation failed: {error}");
                    throw new Exception($"Vertex shader compilation failed: {error}");
                }
                else
                {
                    Console.WriteLine("Vertex shader compiled successfully.");
                }

                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentCode);
                GL.CompileShader(fragmentShader);
                GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fSuccess);
                if (fSuccess == 0)
                {
                    string error = GL.GetShaderInfoLog(fragmentShader);
                    Console.WriteLine($"Fragment shader compilation failed: {error}");
                    throw new Exception($"Fragment shader compilation failed: {error}");
                }
                else
                {
                    Console.WriteLine("Fragment shader compiled successfully.");
                }

                _program = GL.CreateProgram();
                GL.AttachShader(_program, vertexShader);
                GL.AttachShader(_program, fragmentShader);
                GL.LinkProgram(_program);
                GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out int pSuccess);
                if (pSuccess == 0)
                {
                    string error = GL.GetProgramInfoLog(_program);
                    Console.WriteLine($"Shader program linking failed: {error}");
                    throw new Exception($"Shader program linking failed: {error}");
                }
                else
                {
                    Console.WriteLine("Shader program linked successfully.");
                }

                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shaders: {ex.Message}");
                throw;
            }
        }

        public void Use()
        {
            GL.UseProgram(_program);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_program, name);
            if (location == -1)
            {
                Console.WriteLine($"Uniform '{name}' not found in shader.");
            }
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_program);
        }
    }
}