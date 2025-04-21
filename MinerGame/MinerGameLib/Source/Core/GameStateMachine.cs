namespace MinerGameLib.Source.Core
{
    public class GameStateMachine
    {
        private readonly GameManager _gameManager;

        public GameStateMachine(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Update(float deltaTime)
        {
            _gameManager.Update(deltaTime);
        }
    }
}