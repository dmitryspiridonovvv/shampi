using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using MinerGameLib;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGameWF
{
    public class GameForm : GameWindow
    {
        private Game game;

        public GameForm() : base(GameWindowSettings.Default, new NativeWindowSettings
        {
            ClientSize = new OpenTK.Mathematics.Vector2i(1280, 720),
            Title = "Miner Game",
            WindowBorder = WindowBorder.Fixed
        })
        {
            game = new Game(1280, 720);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            game.Initialize();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            game.Update((float)args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            game.Render();
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            game.HandleKeyDown(e.Key);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            game.HandleKeyUp(e.Key);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            game.Dispose();
        }
    }
}