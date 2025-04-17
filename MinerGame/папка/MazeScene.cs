using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGameLib
{
    public class MazeScene : IDisposable
    {
        private char[,] maze;
        private int rows, cols;
        private int playerX, playerY;
        private float cellSize;
        private int vao, vbo;
        private int width, height;

        public MazeScene()
        {
            rows = 10;
            cols = 15;
            maze = new char[rows, cols];
            GenerateMaze();
            FindPlayerPosition();
        }

        private void GenerateMaze()
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (x == 0 || y == 0 || x == cols - 1 || y == rows - 1)
                        maze[y, x] = 'E'; // Стены по краям
                    else
                        maze[y, x] = ' '; // Пустое пространство
                }
            }

            for (int y = 1; y < rows - 1; y++)
            {
                maze[y, 2] = 'E';
                maze[y, 4] = 'E';
                maze[y, 6] = 'E';
                maze[y, 8] = 'E';
                maze[y, 10] = 'E';
                maze[y, 12] = 'E';
            }

            maze[rows - 2, 1] = '.';
        }

        private void FindPlayerPosition()
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (maze[y, x] == '.')
                    {
                        playerX = x;
                        playerY = y;
                        return;
                    }
                }
            }
        }

        public void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Рассчитываем размер клетки
            cellSize = Math.Min((float)width / cols, (float)height / rows);
            Console.WriteLine($"Cell size: {cellSize}"); // Отладка

            // Загружаем текстуры
            Texture.LoadTexture("wall1.png");
            Texture.LoadTexture("miner1.png");

            // Создаём вершины для клетки
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            // Вершины для квадрата с размером cellSize
            float[] vertices = new float[]
            {
                0,        0,         0, 0,
                cellSize, 0,         1, 0,
                cellSize, cellSize, 1, 1,

                0,        0,         0, 0,
                cellSize, cellSize, 1, 1,
                0,        cellSize, 0, 1
            };

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }

        public void Update(float deltaTime)
        {
            // Обновление логики
        }

        public void Render(Shader shader)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Рассчитываем смещение для центрирования
            float mazeWidth = cols * cellSize;
            float mazeHeight = rows * cellSize;
            float offsetX = (width - mazeWidth) / 2;
            float offsetY = (height - mazeHeight) / 2;
            Console.WriteLine($"Maze size: {mazeWidth}x{mazeHeight}, Offset: ({offsetX}, {offsetY})"); // Отладка

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (maze[y, x] == 'E')
                    {
                        Texture.BindTexture("wall1.png");
                    }
                    else if (maze[y, x] == '.')
                    {
                        Texture.BindTexture("miner1.png");
                    }
                    else
                    {
                        continue;
                    }

                    float posX = offsetX + x * cellSize;
                    float posY = offsetY + y * cellSize;
                    Console.WriteLine($"Rendering cell ({x}, {y}) at ({posX}, {posY})"); // Отладка
                    Matrix4 modelMatrix = Matrix4.CreateTranslation(posX, posY, 0);
                    shader.SetMatrix4("uModel", modelMatrix);

                    GL.BindVertexArray(vao);
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                    GL.BindVertexArray(0);
                }
            }
        }

        public void HandleKeyDown(Keys key)
        {
            int newX = playerX;
            int newY = playerY;

            if (key == Keys.Up) newY--;
            if (key == Keys.Down) newY++;
            if (key == Keys.Left) newX--;
            if (key == Keys.Right) newX++;

            if (newX >= 0 && newX < cols && newY >= 0 && newY < rows && maze[newY, newX] != 'E')
            {
                maze[playerY, playerX] = ' ';
                playerX = newX;
                playerY = newY;
                maze[playerY, playerX] = '.';
            }
        }

        public void HandleKeyUp(Keys key)
        {
            // Ничего не делаем
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}