using Stateless;
using System;

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

        public void Update(float deltaTime)
        {
            Console.WriteLine($"GameManager Update: Current State = {_stateMachine.State}");
        }

        public void TransitionTo(string trigger)
        {
            if (_stateMachine.CanFire(trigger))
            {
                Console.WriteLine($"Transitioning to {trigger}");
                _stateMachine.Fire(trigger);
            }
            else
            {
                Console.WriteLine($"Cannot transition with trigger {trigger} from state {_stateMachine.State}");
            }
        }

        public string GetCurrentState()
        {
            return _stateMachine.State;
        }
    }
}