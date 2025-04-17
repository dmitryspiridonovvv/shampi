using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using MinerGame.Core;

namespace MinerGame
{
    class Program
    {
        static void Main()
        {
            var windowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Miner Game",
                WindowBorder = WindowBorder.Fixed
            };

            using (var window = new GameWindow(GameWindowSettings.Default, windowSettings))
            {
                var gameManager = new GameManager(window.Size);
                window.Resize += args => gameManager.UpdateWindowSize(window.Size);
                window.UpdateFrame += args => gameManager.Update((float)args.Time);
                window.RenderFrame += args =>
                {
                    gameManager.Render();
                    window.SwapBuffers();
                };
                window.Run();
            }
        }
    }
}