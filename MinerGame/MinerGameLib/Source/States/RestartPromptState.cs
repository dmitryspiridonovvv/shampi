using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;

namespace MinerGameLib.Source.States
{
    public class RestartPromptState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public RestartPromptState(GameManager gameManager, MenuRenderer menuRenderer)
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
            var bitmap = _menuRenderer.RenderMenu("Restart?");
            // Дополнительная логика рендеринга
        }

        public void HandleInput(string input)
        {
            if (input == "Confirm")
            {
                _gameManager.TransitionTo("Confirm");
            }
        }
    }
}