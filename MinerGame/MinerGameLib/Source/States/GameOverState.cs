using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;

namespace MinerGameLib.Source.States
{
    public class GameOverState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public GameOverState(GameManager gameManager, MenuRenderer menuRenderer)
        {
            _gameManager = gameManager;
            _menuRenderer = menuRenderer;
        }

        public void Update()
        {
            // Логика обновления
        }

        public void Render()
        {
            // Пример рендеринга
            var bitmap = _menuRenderer.RenderMenu("Game Over");
            // Дополнительная логика рендеринга
        }

        public void HandleInput(string input)
        {
            if (input == "Restart")
            {
                _gameManager.TransitionTo("Restart");
            }
        }
    }
}