using MinerGameLib.Source.Core;
using MinerGame.Maze;
using System.Linq;
using MinerGame.Core;

namespace MinerGameLib.Source.States
{
    public class PlayingState
    {
        private readonly GameManager _gameManager;
        private readonly MazeRenderer _mazeRenderer;
        private readonly IMaze _maze;

        public PlayingState(GameManager gameManager, MazeRenderer mazeRenderer, IMaze maze)
        {
            _gameManager = gameManager;
            _mazeRenderer = mazeRenderer;
            _maze = maze;
        }

        public void Update(float deltaTime)
        {
            _maze.Update(deltaTime);

            var miners = _maze.GetMiners();
            if (miners.All(miner => !miner.IsAlive))
            {
                _gameManager.TransitionTo("GameOver");
            }
        }

        public void Render()
        {
            _mazeRenderer.Render(_maze);
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