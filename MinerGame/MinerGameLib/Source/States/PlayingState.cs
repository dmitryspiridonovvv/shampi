using MinerGame.Maze;
using MinerGame.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGame.States
{
    public class PlayingState : GameState
    {
        private readonly GameManager _gameManager;
        private readonly IMaze _maze;
        private readonly MazeRenderer _mazeRenderer;
        private bool _isGameOver;
        private string _winner = string.Empty;

        public PlayingState(GameManager gameManager)
        {
            _gameManager = gameManager;
            var mazeLoader = new MazeLoader();
            _maze = mazeLoader.LoadRandomMaze(
                gameManager.WindowSize,
                gameManager.Config.PlayerSpeed,
                gameManager.Config.BombTimer,
                gameManager.Config.ExplosionRadius,
                gameManager.Config.MineCooldown,
                gameManager.Renderer
            );
            _mazeRenderer = new MazeRenderer(_maze);
        }

        public override void Enter()
        {
            _gameManager.InputHandler.OnKeyDown += HandleKeyDown;
            _gameManager.InputHandler.OnKeyUp += _maze.HandleKeyUp;
        }

        public override void Exit()
        {
            _gameManager.InputHandler.OnKeyDown -= HandleKeyDown;
            _gameManager.InputHandler.OnKeyUp -= _maze.HandleKeyUp;
            _maze.Dispose();
        }

        public override void Update(float deltaTime)
        {
            if (_isGameOver)
            {
                _gameManager.StateMachine.TransitionTo(new GameOverState(_gameManager, _winner));
                return;
            }

            _maze.Update(deltaTime);
            CheckGameOver();
        }

        public override void Render(Renderer renderer)
        {
            _mazeRenderer.Render(renderer);
        }

        private void CheckGameOver()
        {
            var miners = _maze.GetMiners();
            bool player1Alive = miners.Count > 0 && miners[0].IsAlive;
            bool player2Alive = miners.Count > 1 && miners[1].IsAlive;

            if (!player1Alive && !player2Alive)
            {
                _isGameOver = true;
                _winner = "Draw";
            }
            else if (!player1Alive)
            {
                _isGameOver = true;
                _winner = "Player 2";
            }
            else if (!player2Alive)
            {
                _isGameOver = true;
                _winner = "Player 1";
            }
        }

        private void HandleKeyDown(Keys key)
        {
            if (key == Keys.P)
            {
                _gameManager.StateMachine.TransitionTo(new PauseState(_gameManager));
            }
            else
            {
                _maze.HandleKeyDown(key);
            }
        }
    }
}