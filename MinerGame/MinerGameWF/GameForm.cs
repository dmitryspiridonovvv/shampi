using Stateless;

namespace MinerGameLib.Source.Core
{
    public class GameManager
    {
        private readonly StateMachine<string, string> _stateMachine;

        public GameManager()
        {
            _stateMachine = new StateMachine<string, string>("Menu");

            _stateMachine.Configure("Menu")
                .Permit("Start", "Playing")
                .Permit("Settings", "Settings");

            _stateMachine.Configure("Playing")
                .Permit("Pause", "Pause")
                .Permit("GameOver", "GameOver");

            _stateMachine.Configure("Pause")
                .Permit("Resume", "Playing")
                .Permit("Menu", "Menu");

            _stateMachine.Configure("GameOver")
                .Permit("Restart", "RestartPrompt");

            _stateMachine.Configure("RestartPrompt")
                .Permit("Confirm", "Menu");

            _stateMachine.Configure("Settings")
                .Permit("Back", "Menu");
        }

        public void Update()
        {
            // Логика обновления игры
        }

        public void Render()
        {
            // Логика рендеринга игры
        }

        public void TransitionTo(string trigger)
        {
            if (_stateMachine.CanFire(trigger))
            {
                _stateMachine.Fire(trigger);
            }
        }

        public string GetCurrentState()
        {
            return _stateMachine.State;
        }
    }
}