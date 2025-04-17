using MinerGameLib.Source.Core;

namespace MinerGameLib.Source.States
{
    public class PlayingState
    {
        private readonly GameManager _gameManager;

        public PlayingState(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Update()
        {
            // Логика игры
        }

        public void Render()
        {
            _gameManager.Render();
        }

        public void HandleInput(string input)
        {
            if (input == "Pause")
            {
                _gameManager.TransitionTo("Pause");
            }
            else if (input == "GameOver")
            {
                _gameManager.TransitionTo("GameOver");
            }
        }
    }
}