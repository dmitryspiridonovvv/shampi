using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;

namespace MinerGameLib.Source.States
{
    public class MenuState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public MenuState(GameManager gameManager, MenuRenderer menuRenderer)
        {
            _gameManager = gameManager;
            _menuRenderer = menuRenderer;
        }

        public void Update()
        {
            // Логика обновления меню
        }

        public void Render()
        {
            // Пример рендеринга
            var bitmap = _menuRenderer.RenderMenu("Main Menu");
            // Дополнительная логика рендеринга
        }

        public void HandleInput(string input)
        {
            if (input == "Start")
            {
                _gameManager.TransitionTo("Start");
            }
            else if (input == "Settings")
            {
                _gameManager.TransitionTo("Settings");
            }
        }
    }
}