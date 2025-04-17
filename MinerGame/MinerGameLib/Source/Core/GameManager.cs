using MinerGame.States;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinerGame.Core
{
    public class GameManager
    {
        public Renderer Renderer { get; }
        public Vector2i WindowSize { get; private set; }
        public InputHandler InputHandler { get; }
        public StateMachine StateMachine { get; }

        public GameManager(Vector2i windowSize)
        {
            WindowSize = windowSize;
            Renderer = new Renderer(windowSize);
            InputHandler = new InputHandler();
            StateMachine = new StateMachine();
            StateMachine.TransitionTo(new MenuState(this));

            // Установка цвета фона
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Чёрный фон
        }

        public void UpdateWindowSize(Vector2i newSize)
        {
            WindowSize = newSize;
            Renderer.UpdateWindowSize(newSize);
            GL.Viewport(0, 0, newSize.X, newSize.Y);
        }

        public void Update(float deltaTime)
        {
            StateMachine.Update(deltaTime);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            StateMachine.Render(Renderer);
        }
    }
}