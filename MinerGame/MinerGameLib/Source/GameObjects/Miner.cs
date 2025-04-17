using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MinerGame.Core;
using MinerGame.Maze;
using System;

namespace MinerGame.GameObjects
{
    public class Miner : GameObject, IUpdatable
    {
        private readonly bool _isPlayer1;
        private int _health;
        private float _speed;
        private Vector2 _direction;
        private readonly float _cellSize;
        private readonly float _mineCooldown;
        private float _lastMineTime;
        private readonly Renderer _minerRenderer;

        public bool IsAlive { get; set; }
        public int ExplosionPower { get; private set; } = 1;
        public float ExplosionRadius { get; private set; } = 50f;

        public Miner(Vector2 position, string texturePath, bool isPlayer1, int health, float cellSize, float speed, Renderer renderer, float mineCooldown)
            : base(position, texturePath, cellSize, renderer)
        {
            _isPlayer1 = isPlayer1;
            _health = health;
            IsAlive = true;
            _speed = speed;
            _cellSize = cellSize;
            _mineCooldown = mineCooldown;
            _minerRenderer = renderer;
            _lastMineTime = float.MinValue;
        }

        void IUpdatable.Update(float deltaTime)
        {
            // Пустая реализация, логика в Update(float, IMaze)
        }

        public void Update(float deltaTime, IMaze maze)
        {
            if (!IsAlive) return;

            var newPosition = Position + _direction * _speed * deltaTime;
            if (!maze.IsCollision(newPosition, _cellSize, _cellSize))
            {
                Position = newPosition;
            }
        }

        public void HandleKeyDown(Keys key, IMaze maze, float bombTimer, float explosionRadius)
        {
            if (!IsAlive) return;

            if (_isPlayer1)
            {
                switch (key)
                {
                    case Keys.W: _direction.Y = 1; break;
                    case Keys.S: _direction.Y = -1; break;
                    case Keys.A: _direction.X = -1; break;
                    case Keys.D: _direction.X = 1; break;
                    case Keys.Space:
                        PlaceMine(maze, bombTimer, explosionRadius);
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.Up: _direction.Y = 1; break;
                    case Keys.Down: _direction.Y = -1; break;
                    case Keys.Left: _direction.X = -1; break;
                    case Keys.Right: _direction.X = 1; break; // Исправлено BREAK на break
                    case Keys.Enter:
                        PlaceMine(maze, bombTimer, explosionRadius);
                        break;
                }
            }
        }

        public void HandleKeyUp(Keys key)
        {
            if (_isPlayer1)
            {
                switch (key)
                {
                    case Keys.W:
                    case Keys.S: _direction.Y = 0; break;
                    case Keys.A:
                    case Keys.D: _direction.X = 0; break;
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.Up:
                    case Keys.Down: _direction.Y = 0; break;
                    case Keys.Left:
                    case Keys.Right: _direction.X = 0; break;
                }
            }
        }

        private void PlaceMine(IMaze maze, float bombTimer, float explosionRadius)
        {
            float currentTime = (float)GLFW.GetTime();
            if (currentTime - _lastMineTime >= _mineCooldown)
            {
                var mine = new Mine(Position, "Resources/Textures/mine.png", _cellSize, ExplosionPower, explosionRadius, bombTimer, _minerRenderer);
                maze.AddMine(mine);
                _lastMineTime = currentTime;
            }
        }

        public void ApplyPrize(Prize prize)
        {
            switch (prize.Effect)
            {
                case 1: _speed += 10.0f; break;
                case 2: ExplosionPower += 1; break;
                case -1: IsAlive = false; break;
            }
        }

        public void Reset(Vector2 initialPosition)
        {
            Position = initialPosition;
            IsAlive = true;
            _health = 1;
            _speed = 100.0f;
            ExplosionPower = 1;
            ExplosionRadius = 50f;
            _direction = Vector2.Zero;
            _lastMineTime = float.MinValue;
        }
    }
}