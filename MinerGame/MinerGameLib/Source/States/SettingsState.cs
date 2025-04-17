using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;
using MinerGameLib.Source.States;

namespace MinerGameLib.Source.States
{
    public class SettingsState
    {
        private readonly GameManager _gameManager;
        private readonly MenuRenderer _menuRenderer;

        public SettingsState(GameManager gameManager, MenuRenderer menuRenderer)
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
            var bitmap = _menuRenderer.RenderMenu("Settings");
            // Дополнительная логика рендеринга
        }

        public void HandleInput(string input)
        {
            if (input == "Back")
            {
                _gameManager.TransitionTo("Back");
            }
        }

        public void SwitchToMenu()
        {
            // Исправленный вызов конструктора MenuState
            var menuState = new MenuState(_gameManager, _menuRenderer);
            // Логика переключения на меню (если требуется)
        }
    }
}