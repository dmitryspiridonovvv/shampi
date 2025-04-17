using MinerGame.States;
using System;

namespace MinerGame.Core
{
    public class GameStateMachine
    {
        private GameState? _currentState;
        private readonly GameManager _gameManager;

        public GameStateMachine(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void TransitionTo(GameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        public void Render(Renderer renderer)
        {
            _currentState?.Render(renderer);
        }
    }
}