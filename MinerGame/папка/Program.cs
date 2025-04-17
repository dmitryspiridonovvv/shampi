using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGameWF
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1280, 720),
                Title = "Miner Game",
                WindowBorder = WindowBorder.Fixed,
                WindowState = WindowState.Normal,
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var window = new GameWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                var game = new MinerGameLib.Game(1280, 720);
                window.Load += () =>
                {
                    GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    game.Initialize();
                };

                window.UpdateFrame += (args) =>
                {
                    game.Update((float)args.Time);
                };

                window.RenderFrame += (args) =>
                {
                    game.Render();
                    window.SwapBuffers();
                };

                window.KeyDown += (args) =>
                {
                    Console.WriteLine($"KeyDown: {args.Key}"); // Отладка
                    game.HandleKeyDown(args.Key);
                };

                window.KeyUp += (args) =>
                {
                    game.HandleKeyUp(args.Key);
                };

                window.Run();
                window.Closing += (args) =>
                {
                    game.Dispose();
                };
            }
        }
    }
}