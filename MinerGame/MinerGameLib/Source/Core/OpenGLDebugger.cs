using OpenTK.Graphics.OpenGL4;

namespace MinerGame.Core
{
    /// <summary>
    /// Проверяет ошибки OpenGL.
    /// </summary>
    public static class OpenGLDebugger
    {
        public static void CheckErrors()
        {
            int error = (int)GL.GetError();
            if (error != 0)
            {
                Console.WriteLine($"Ошибка OpenGL: {error}");
            }
        }
    }
}