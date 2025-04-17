using MinerGame.Core;
using MinerGame.UI;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGame.States
{
    public class GameOverState : GameState
    {
        private readonly GameManager _gameManager;
        private readonly string _winner;
        private readonly MenuRenderer _menuRenderer;

        public GameOverState(GameManager gameManager, string winner)
        {
            _gameManager = gameManager;
            _winner = winner;
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
        }

        public override void Render(Renderer renderer)
        {
            Console.WriteLine("Rendering GameOverState");
            var fontRenderer = new FontRenderer(renderer, "Resources/Fonts/Agitpropc.otf", 16);
            string text = $"Game Over\nWinner: {_winner}\nPress Escape for Menu\nR to Restart";
            var lines = text.Split('\n');
            float lineHeight = 20;
            float y = _gameManager.WindowSize.Y / 2 - (lines.Length - 1) * lineHeight / 2;

            foreach (var line in lines)
            {
                var textSize = new Vector2(line.Length * 10, 16);
                var position = new Vector2((_gameManager.WindowSize.X - textSize.X) / 2, y);
                fontRenderer.RenderText(line, position, 1.0f);
                y += lineHeight;
            }

            fontRenderer.Dispose();
        }

        private void HandleKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                _gameManager.StateMachine.TransitionTo(new MenuState(_gameManager));
            }
            else if (key == Keys.R)
            {
                _gameManager.StateMachine.TransitionTo(new RestartPromptState(_gameManager));
            }
        }
    }
}