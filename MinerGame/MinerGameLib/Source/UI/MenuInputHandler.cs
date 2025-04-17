using MinerGame.Core;
using OpenTK.Mathematics;
using System;

namespace MinerGame.UI
{
    public class MenuInputHandler
    {
        private readonly InputHandler _inputHandler;
        private readonly bool _isSettings;
        private readonly bool _isPause;
        private readonly bool _isGameOver;
        private readonly bool _isRestartPrompt;

        public event Action? OnStart;
        public event Action? OnSettings;
        public event Action? OnExit;
        public event Action? OnBack;
        public event Action? OnApply;
        public event Action? OnResume;
        public event Action? OnRestart;
        public event Action? OnMainMenu;
        public event Action? OnYes;
        public event Action? OnNo;

        public MenuInputHandler(InputHandler inputHandler, bool isSettings = false, bool isPause = false, bool isGameOver = false, bool isRestartPrompt = false)
        {
            _inputHandler = inputHandler;
            _isSettings = isSettings;
            _isPause = isPause;
            _isGameOver = isGameOver;
            _isRestartPrompt = isRestartPrompt;
            _inputHandler.OnMouseClick += HandleMouseClick;
        }

        public void Update()
        {
            // Обновление состояния мыши
        }

        private void HandleMouseClick(Vector2 position)
        {
            if (_isSettings)
            {
                float applyX = 540, applyY = 460, buttonWidth = 200, buttonHeight = 80;
                float backX = 540, backY = 360;

                if (position.X >= applyX && position.X <= applyX + buttonWidth &&
                    position.Y >= applyY && position.Y <= applyY + buttonHeight)
                {
                    OnApply?.Invoke();
                }
                else if (position.X >= backX && position.X <= backX + buttonWidth &&
                         position.Y >= backY && position.Y <= backY + buttonHeight)
                {
                    OnBack?.Invoke();
                }
            }
            else if (_isPause)
            {
                float resumeX = 540, resumeY = 460, buttonWidth = 200, buttonHeight = 80;
                float settingsX = 540, settingsY = 360;
                float exitX = 540, exitY = 260;

                if (position.X >= resumeX && position.X <= resumeX + buttonWidth &&
                    position.Y >= resumeY && position.Y <= resumeY + buttonHeight)
                {
                    OnResume?.Invoke();
                }
                else if (position.X >= settingsX && position.X <= settingsX + buttonWidth &&
                         position.Y >= settingsY && position.Y <= settingsY + buttonHeight)
                {
                    OnSettings?.Invoke();
                }
                else if (position.X >= exitX && position.X <= exitX + buttonWidth &&
                         position.Y >= exitY && position.Y <= exitY + buttonHeight)
                {
                    OnExit?.Invoke();
                }
            }
            else if (_isGameOver)
            {
                float restartX = 540, restartY = 360, buttonWidth = 200, buttonHeight = 80;
                float mainMenuX = 540, mainMenuY = 260;

                if (position.X >= restartX && position.X <= restartX + buttonWidth &&
                    position.Y >= restartY && position.Y <= restartY + buttonHeight)
                {
                    OnRestart?.Invoke();
                }
                else if (position.X >= mainMenuX && position.X <= mainMenuX + buttonWidth &&
                         position.Y >= mainMenuY && position.Y <= mainMenuY + buttonHeight)
                {
                    OnMainMenu?.Invoke();
                }
            }
            else if (_isRestartPrompt)
            {
                float yesX = 540, yesY = 360, buttonWidth = 200, buttonHeight = 80;
                float noX = 540, noY = 260;

                if (position.X >= yesX && position.X <= yesX + buttonWidth &&
                    position.Y >= yesY && position.Y <= yesY + buttonHeight)
                {
                    OnYes?.Invoke();
                }
                else if (position.X >= noX && position.X <= noX + buttonWidth &&
                         position.Y >= noY && position.Y <= noY + buttonHeight)
                {
                    OnNo?.Invoke();
                }
            }
            else
            {
                float playButtonX = 540, playButtonY = 280, playButtonWidth = 200, playButtonHeight = 80;
                float settingsX = 50, settingsY = 606, settingsSize = 64;
                float quitX = 134, quitY = 606, quitSize = 64;

                if (position.X >= playButtonX && position.X <= playButtonX + playButtonWidth &&
                    position.Y >= playButtonY && position.Y <= playButtonY + playButtonHeight)
                {
                    OnStart?.Invoke();
                }
                else if (position.X >= settingsX && position.X <= settingsX + settingsSize &&
                         position.Y >= settingsY && position.Y <= settingsY + settingsSize)
                {
                    OnSettings?.Invoke();
                }
                else if (position.X >= quitX && position.X <= quitX + quitSize &&
                         position.Y >= quitY && position.Y <= quitY + quitSize)
                {
                    OnExit?.Invoke();
                }
            }
        }
    }
}