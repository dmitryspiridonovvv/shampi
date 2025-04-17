using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGameLib
{
    public class Game : IDisposable
    {
        private readonly int width, height;
        private MazeScene mazeScene;
        private Background background;
        private Menu menu;
        private Shader? shader;
        private Matrix4 projectionMatrix;
        private GameState gameState;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
            mazeScene = new MazeScene();
            background = new Background(width, height);
            menu = new Menu(width, height);
            gameState = GameState.Menu;
        }

        public void Initialize()
        {
            shader = new Shader("Shader.vert", "Shader.frag");
            projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, -1, 1);
            mazeScene.Initialize(width, height);
            menu.Initialize();
            LoadResources();
        }

        private void LoadResources()
        {
            Texture.LoadTexture("background.png");
            Texture.LoadTexture("miner1.png");
            Texture.LoadTexture("miner2.png");
            Texture.LoadTexture("mine.png");
            Texture.LoadTexture("prize1.png");
            Texture.LoadTexture("prize2.png");
            Texture.LoadTexture("prize3.png");
            Texture.LoadTexture("wall1.png");
            Texture.LoadTexture("wall2.png");
            Texture.LoadTexture("wall3.png");
            Texture.LoadTexture("wall4.png");
            Texture.LoadTexture("wall5.png");
        }

        public void Update(float deltaTime)
        {
            if (gameState == GameState.Playing)
            {
                mazeScene.Update(deltaTime);
            }
        }

        public void Render()
        {
            GL.Viewport(0, 0, width, height);
            Console.WriteLine("Clearing buffer"); // Отладка
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader?.Use();
            shader?.SetMatrix4("uProjection", projectionMatrix);

            background.Render(shader!);

            switch (gameState)
            {
                case GameState.Menu:
                    menu.Render(shader!);
                    break;
                case GameState.Playing:
                    mazeScene.Render(shader!);
                    break;
                case GameState.Settings:
                    menu.RenderSettings(shader!);
                    break;
            }

            var error = GL.GetError();
            if (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error in Game.Render: {error}");
            }
        }

        public void HandleKeyDown(Keys key)
        {
            Console.WriteLine($"Game.HandleKeyDown: {key}, State: {gameState}"); // Отладка
            switch (gameState)
            {
                case GameState.Menu:
                    HandleMenuInput(key);
                    break;
                case GameState.Playing:
                    mazeScene.HandleKeyDown(key);
                    break;
                case GameState.Settings:
                    HandleSettingsInput(key);
                    break;
            }
        }

        public void HandleKeyUp(Keys key)
        {
            if (gameState == GameState.Playing)
            {
                mazeScene.HandleKeyUp(key);
            }
        }

        private void HandleMenuInput(Keys key)
        {
            Console.WriteLine($"HandleMenuInput: {key}");
            if (key == Keys.Down)
                menu.MoveDown();
            else if (key == Keys.Up)
                menu.MoveUp();
            else if (key == Keys.Enter)
            {
                Console.WriteLine($"Enter pressed, SelectedOption: {menu.SelectedOption}");
                switch (menu.SelectedOption)
                {
                    case 0: // Start (вверху)
                        gameState = GameState.Playing;
                        break;
                    case 1: // Settings (посредине)
                        gameState = GameState.Settings;
                        break;
                    case 2: // Exit (внизу)
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private void HandleSettingsInput(Keys key)
        {
            if (key == Keys.Escape)
            {
                gameState = GameState.Menu;
            }
        }

        public void Dispose()
        {
            Texture.DisposeAll();
            background.Dispose();
            mazeScene.Dispose();
            menu.Dispose();
            shader?.Dispose();
        }
    }
}