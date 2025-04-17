using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MinerGame.Core;
using System;

namespace MinerGameWF
{
    public class GameForm : GameWindow
    {
        private GameManager? _gameManager;
        private readonly InputHandler _inputHandler;
        private float _lastFrameTime;

        public GameForm(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _inputHandler = new InputHandler();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _gameManager = new GameManager(new Vector2i(Size.X, Size.Y));
            _lastFrameTime = (float)GLFW.GetTime();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _inputHandler.Resize(new Vector2i(e.Width, e.Height));
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _gameManager?.Dispose();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            var pos = new Vector2(MousePosition.X, Size.Y - MousePosition.Y);
            _inputHandler.MouseClick(pos);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            _inputHandler.KeyDown(e.Key);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            _inputHandler.KeyUp(e.Key);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (_gameManager == null) return;

            float currentTime = (float)GLFW.GetTime();
            float deltaTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;

            _gameManager.Update(deltaTime);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            if (_gameManager == null) return;

            GL.Clear(ClearBufferMask.ColorBufferBit);
            _gameManager.Render();
            SwapBuffers();
        }
    }
}