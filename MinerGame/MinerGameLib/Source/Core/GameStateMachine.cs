using MinerGameLib.Source.Core;

namespace MinerGameLib.Source.Core
{
    public class GameStateMachine
    {
        private readonly GameManager _gameManager;

        public GameStateMachine(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Update()
        {
            _gameManager.Update();
        }

        public void Render()
        {
            _gameManager.Render();
        }
    }
}