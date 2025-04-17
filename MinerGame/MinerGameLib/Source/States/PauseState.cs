using MinerGame.Core;
using MinerGame.UI;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGame.States
{
    public class PauseState : GameState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public PauseState(GameManager gameManager)
        {
            _gameManager = gameManager;
            _menuRenderer = new MenuRenderer(gameManager.Renderer, gameManager.WindowSize);
        }

        public override void Enter()
        {
            _gameManager.InputHandler.OnKeyDown += HandleKeyDown;
        }

        public override void Exit()
        {
            _gameManager.InputHandler.OnKeyDown -= HandleKeyDown;
            _menuRenderer.Dispose();
        }

        public override void Update(float deltaTime)
        {
            // No updates needed
        }

        public override void Render(Renderer renderer)
        {
            _menuRenderer.Render(renderer);
        }

        private void HandleKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                _gameManager.StateMachine.TransitionTo(new PlayingState(_gameManager));
            }
            else if (key == Keys.M)
            {
                _gameManager.StateMachine.TransitionTo(new MenuState(_gameManager));
            }
        }
    }
}