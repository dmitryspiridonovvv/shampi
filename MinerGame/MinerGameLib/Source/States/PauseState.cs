using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;

namespace MinerGameLib.Source.States
{
    public class PauseState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public PauseState(GameManager gameManager, MenuRenderer menuRenderer)
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
            var bitmap = _menuRenderer.RenderMenu("Paused");
            // Дополнительная логика рендеринга
        }

        public void HandleInput(string input)
        {
            if (input == "Resume")
            {
                _gameManager.TransitionTo("Resume");
            }
            else if (input == "Menu")
            {
                _gameManager.TransitionTo("Menu");
            }
        }
    }
}