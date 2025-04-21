using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using MinerGame.Core;
using MinerGame.Maze;
using MinerGameLib.Source.Core;
using MinerGameLib.Source.UI;
using MinerGameLib.Source.States;
using System;
using System.IO;
using System.Text.Json;
using System.Runtime.Versioning;
using MinerGame.UI;

namespace MinerGameWF
{
    [SupportedOSPlatform("windows")]
    public class GameWindow : OpenTK.Windowing.Desktop.GameWindow
    {
        private GameManager? _gameManager;
        private Renderer? _renderer;
        private InputHandler? _inputHandler;
        private IMaze? _maze;
        private MazeRenderer? _mazeRenderer;
        private MenuInputHandler? _menuInputHandler;
        private MenuRenderer? _menuRenderer;
        private MenuState? _menuState;
        private PlayingState? _playingState;
        private PauseState? _pauseState;
        private GameOverState? _gameOverState;
        private RestartPromptState? _restartPromptState;
        private SettingsState? _settingsState;
        private GameConfig? _config;

        private static readonly string[] ConfigPaths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gameconfig.json"),
            @"W:\education\Курсовая\MinerGame\MinerGameLib\Config\Gameconfig.json"
        };

        private static readonly string[] FontPaths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Agitpropc.otf"),
            @"W:\education\Курсовая\MinerGame\MinerGameLib\Resources\Fonts\Agitpropc.otf"
        };

        public GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Console.WriteLine("[GameWindow.OnLoad] Starting initialization...");

            // Загрузка конфигурации
            try
            {
                string configPath = null;
                foreach (var path in ConfigPaths)
                {
                    Console.WriteLine($"[GameWindow.OnLoad] Checking Gameconfig.json at: {path}");
                    if (File.Exists(path))
                    {
                        configPath = path;
                        break;
                    }
                }

                if (configPath == null)
                {
                    Console.WriteLine("[GameWindow.OnLoad] Error: Gameconfig.json not found at any of the following paths:");
                    foreach (var path in ConfigPaths)
                        Console.WriteLine($"  - {path}");
                    Console.WriteLine("[GameWindow.OnLoad] Please ensure Gameconfig.json exists in W:\\education\\Курсовая\\MinerGame\\MinerGameLib\\Config and is copied to W:\\education\\Курсовая\\MinerGame\\MinerGameWF\\bin\\Debug\\net8.0.");
                    Close();
                    return;
                }

                Console.WriteLine($"[GameWindow.OnLoad] Loading Gameconfig.json from: {configPath}");
                string configJson;
                try
                {
                    configJson = File.ReadAllText(configPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GameWindow.OnLoad] Error reading Gameconfig.json: {ex.Message}");
                    Console.WriteLine($"[GameWindow.OnLoad] Stack trace: {ex.StackTrace}");
                    Close();
                    return;
                }

                Console.WriteLine("[GameWindow.OnLoad] Deserializing Gameconfig.json...");
                try
                {
                    _config = JsonSerializer.Deserialize<GameConfig>(configJson) ?? throw new InvalidOperationException("Deserialization returned null.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GameWindow.OnLoad] Error deserializing Gameconfig.json: {ex.Message}");
                    Console.WriteLine($"[GameWindow.OnLoad] Stack trace: {ex.StackTrace}");
                    Console.WriteLine("[GameWindow.OnLoad] Please ensure Gameconfig.json has the correct format (e.g., valid JSON with DefaultResolution, PlayerSpeed, etc.).");
                    Close();
                    return;
                }
                Console.WriteLine("[GameWindow.OnLoad] Gameconfig.json loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameWindow.OnLoad] Error loading config: {ex.Message}");
                Console.WriteLine($"[GameWindow.OnLoad] Stack trace: {ex.StackTrace}");
                Close();
                return;
            }

            // Настройка OpenGL
            try
            {
                Console.WriteLine("[GameWindow.OnLoad] Setting up OpenGL...");
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
                Console.WriteLine("[GameWindow.OnLoad] OpenGL setup completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameWindow.OnLoad] Error setting up OpenGL: {ex.Message}");
                Console.WriteLine($"[GameWindow.OnLoad] Stack trace: {ex.StackTrace}");
                Close();
                return;
            }

            // Инициализация компонентов
            try
            {
                Console.WriteLine("[GameWindow.OnLoad] Initializing components...");
                _renderer = new Renderer(new Vector2i(_config.DefaultResolution!.X, _config.DefaultResolution.Y));
                _inputHandler = new InputHandler();
                _gameManager = new GameManager();
                Console.WriteLine("[GameWindow.OnLoad] Creating Maze...");
                _maze = new Maze(
                    new Vector2i(_config.DefaultResolution.X, _config.DefaultResolution.Y),
                    _config.PlayerSpeed,
                    _config.BombTimer,
                    _config.ExplosionRadius,
                    _config.MineCooldown,
                    _renderer,
                    _config.PrizeSpawnInterval
                );
                _mazeRenderer = new MazeRenderer(_maze);

                string fontPath = null;
                foreach (var path in FontPaths)
                {
                    Console.WriteLine($"[GameWindow.OnLoad] Checking font at: {path}");
                    if (File.Exists(path))
                    {
                        fontPath = path;
                        break;
                    }
                }

                if (fontPath == null)
                {
                    Console.WriteLine("[GameWindow.OnLoad] Error: Agitpropc.otf not found at any of the following paths:");
                    foreach (var path in FontPaths)
                        Console.WriteLine($"  - {path}");
                    Console.WriteLine("[GameWindow.OnLoad] Please ensure Agitpropc.otf exists in W:\\education\\Курсовая\\MinerGame\\MinerGameLib\\Resources\\Fonts and is copied to W:\\education\\Курсовая\\MinerGame\\MinerGameWF\\bin\\Debug\\net8.0.");
                    Close();
                    return;
                }

                Console.WriteLine($"[GameWindow.OnLoad] Loading font from: {fontPath}");
                _menuRenderer = new MenuRenderer(fontPath, 24f);
                _menuInputHandler = new MenuInputHandler(_inputHandler);

                // Инициализация состояний
                Console.WriteLine("[GameWindow.OnLoad] Initializing states...");
                _menuState = new MenuState(_gameManager, _menuRenderer, _renderer);
                _playingState = new PlayingState(_gameManager, _mazeRenderer, _maze);
                _pauseState = new PauseState(_gameManager, _menuRenderer, _renderer);
                _gameOverState = new GameOverState(_gameManager, _menuRenderer, _renderer);
                _restartPromptState = new RestartPromptState(_gameManager, _menuRenderer, _renderer);
                _settingsState = new SettingsState(_gameManager, _menuRenderer, _renderer);

                // Подключение ввода
                _inputHandler.OnKeyDown += key => _maze?.HandleKeyDown(key);
                _inputHandler.OnKeyUp += key => _maze?.HandleKeyUp(key);

                // Подключение событий меню
                _menuInputHandler.OnStart += () => _gameManager.TransitionTo("Start");
                _menuInputHandler.OnSettings += () => _gameManager.TransitionTo("Settings");
                _menuInputHandler.OnResume += () => _gameManager.TransitionTo("Resume");
                _menuInputHandler.OnMainMenu += () => _gameManager.TransitionTo("Menu");
                _menuInputHandler.OnRestart += () => _gameManager.TransitionTo("Restart");
                _menuInputHandler.OnYes += () => _gameManager.TransitionTo("Confirm");
                _menuInputHandler.OnBack += () => _gameManager.TransitionTo("Back");

                Console.WriteLine("[GameWindow.OnLoad] GameWindow initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameWindow.OnLoad] Error initializing components: {ex.Message}");
                Console.WriteLine($"[GameWindow.OnLoad] Stack trace: {ex.StackTrace}");
                Close();
                return;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            try
            {
                string currentState = _gameManager?.GetCurrentState() ?? "None";
                Console.WriteLine($"[GameWindow.OnRenderFrame] Rendering state: {currentState}");
                switch (currentState)
                {
                    case "Menu":
                        _menuState?.Render();
                        break;
                    case "Playing":
                        _playingState?.Render();
                        break;
                    case "Pause":
                        _pauseState?.Render();
                        break;
                    case "GameOver":
                        _gameOverState?.Render();
                        break;
                    case "RestartPrompt":
                        _restartPromptState?.Render();
                        break;
                    case "Settings":
                        _settingsState?.Render();
                        break;
                    default:
                        Console.WriteLine("[GameWindow.OnRenderFrame] No valid state to render");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameWindow.OnRenderFrame] Error: {ex.Message}");
                Console.WriteLine($"[GameWindow.OnRenderFrame] Stack trace: {ex.StackTrace}");
            }

            Context.SwapBuffers();
            OpenGLDebugger.CheckErrors();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float deltaTime = (float)args.Time;
            try
            {
                _gameManager?.Update(deltaTime);

                switch (_gameManager?.GetCurrentState())
                {
                    case "Menu":
                        _menuState?.Update();
                        break;
                    case "Playing":
                        _playingState?.Update(deltaTime);
                        break;
                    case "Pause":
                        _pauseState?.Update();
                        break;
                    case "GameOver":
                        _gameOverState?.Update();
                        break;
                    case "RestartPrompt":
                        _restartPromptState?.Update();
                        break;
                    case "Settings":
                        _settingsState?.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameWindow.OnUpdateFrame] Error: {ex.Message}");
                Console.WriteLine($"[GameWindow.OnUpdateFrame] Stack trace: {ex.StackTrace}");
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            _inputHandler?.KeyDown(e.Key);

            if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape)
            {
                if (_gameManager?.GetCurrentState() == "Playing")
                    _gameManager.TransitionTo("Pause");
                else if (_gameManager?.GetCurrentState() == "Pause")
                    _gameManager.TransitionTo("Resume");
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            _inputHandler?.KeyUp(e.Key);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (_inputHandler != null)
            {
                var mouseState = MouseState;
                _inputHandler.MouseClick(new Vector2(mouseState.X, mouseState.Y));
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            if (_renderer != null)
                _renderer.UpdateWindowSize(new Vector2i(e.Width, e.Height));
            if (_inputHandler != null)
                _inputHandler.Resize(new Vector2i(e.Width, e.Height));
        }

        protected override void OnUnload()
        {
            Console.WriteLine("[GameWindow.OnUnload] Disposing resources...");
            _renderer?.Dispose();
            _maze?.Dispose();
            _menuRenderer?.Dispose();
            base.OnUnload();
        }
    }
}