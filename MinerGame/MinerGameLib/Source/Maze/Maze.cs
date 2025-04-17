using OpenTK.Mathematics;
using MinerGame.Core;
using MinerGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinerGame.Maze
{
    public class Maze : IMaze
    {
        private readonly List<Wall> _walls = new();
        private readonly List<Mine> _mines = new();
        private readonly List<Prize> _prizes = new();
        private Miner? _player1;
        private Miner? _player2;
        private readonly Vector2i _windowSize;
        private readonly float _cellSize;
        private readonly Random _random = new();
        private readonly PrizeFactory _prizeFactory;
        private readonly float _playerSpeed;
        private readonly float _bombTimer;
        private readonly float _explosionRadius;
        private readonly float _mineCooldown;
        private float _lastPrizeSpawnTime;
        private Vector2 _initialPlayer1Position;
        private Vector2 _initialPlayer2Position;
        private readonly Renderer _renderer;

        public IReadOnlyList<Miner> Miners
        {
            get
            {
                var miners = new List<Miner>();
                if (_player1 != null) miners.Add(_player1);
                if (_player2 != null) miners.Add(_player2);
                return miners;
            }
        }

        public Maze(Vector2i windowSize, float playerSpeed, float bombTimer, float explosionRadius, float mineCooldown, Renderer renderer)
        {
            _windowSize = windowSize;
            _cellSize = 50f;
            _playerSpeed = playerSpeed;
            _bombTimer = bombTimer;
            _explosionRadius = explosionRadius;
            _mineCooldown = mineCooldown;
            _renderer = renderer;
            _prizeFactory = new PrizeFactory(_cellSize, renderer);
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            _walls.Clear();
            _mines.Clear();
            _prizes.Clear();

            _initialPlayer1Position = new Vector2(_cellSize, _cellSize);
            _initialPlayer2Position = new Vector2(_windowSize.X - 2 * _cellSize, _windowSize.Y - 2 * _cellSize);

            _player1 = new Miner(_initialPlayer1Position, "Resources/Textures/player1.png", true, 1, _cellSize, _playerSpeed, _renderer, _mineCooldown);
            _player2 = new Miner(_initialPlayer2Position, "Resources/Textures/player2.png", false, 1, _cellSize, _playerSpeed, _renderer, _mineCooldown);

            for (int x = 0; x < _windowSize.X; x += (int)_cellSize)
            {
                for (int y = 0; y < _windowSize.Y; y += (int)_cellSize)
                {
                    if (_random.NextDouble() < 0.2 && (x > _cellSize * 2 || y > _cellSize * 2) && (x < _windowSize.X - _cellSize * 3 || y < _windowSize.Y - _cellSize * 3))
                    {
                        _walls.Add(new Wall(new Vector2(x, y), "Resources/Textures/wall.png", _cellSize, _renderer));
                    }
                }
            }
        }

        public void Update(float deltaTime)
        {
            if (_player1 != null) _player1.Update(deltaTime, this);
            if (_player2 != null) _player2.Update(deltaTime, this);

            UpdateMines(deltaTime);
            CheckCollisions();
            SpawnPrizes(deltaTime);
        }

        public bool IsCollision(Vector2 position, float width, float height)
        {
            foreach (var wall in _walls)
            {
                if (IsBoxCollision(position, width, height, wall.Position, _cellSize, _cellSize))
                    return true;
            }

            if (_player1 != null && IsBoxCollision(position, width, height, _player1.Position, _cellSize, _cellSize))
                return true;
            if (_player2 != null && IsBoxCollision(position, width, height, _player2.Position, _cellSize, _cellSize))
                return true;

            return position.X < 0 || position.Y < 0 || position.X + width > _windowSize.X || position.Y + height > _windowSize.Y;
        }

        private bool IsBoxCollision(Vector2 pos1, float w1, float h1, Vector2 pos2, float w2, float h2)
        {
            return pos1.X < pos2.X + w2 &&
                   pos1.X + w1 > pos2.X &&
                   pos1.Y < pos2.Y + h2 &&
                   pos1.Y + h1 > pos2.Y;
        }

        private void CheckCollisions()
        {
            if (_player1 != null)
            {
                foreach (var prize in _prizes.ToList())
                {
                    if (IsBoxCollision(_player1.Position, _cellSize, _cellSize, prize.Position, _cellSize, _cellSize))
                    {
                        _player1.ApplyPrize(prize);
                        _prizes.Remove(prize);
                    }
                }
            }

            if (_player2 != null)
            {
                foreach (var prize in _prizes.ToList())
                {
                    if (IsBoxCollision(_player2.Position, _cellSize, _cellSize, prize.Position, _cellSize, _cellSize))
                    {
                        _player2.ApplyPrize(prize);
                        _prizes.Remove(prize);
                    }
                }
            }

            if (_player1 != null)
            {
                foreach (var mine in _mines.ToList())
                {
                    if (mine.HasExploded)
                    {
                        if (IsBoxCollision(_player1.Position, _cellSize, _cellSize, mine.Position, mine.ExplosionRadius, mine.ExplosionRadius))
                        {
                            _player1.IsAlive = false;
                        }
                    }
                }
            }

            if (_player2 != null)
            {
                foreach (var mine in _mines.ToList())
                {
                    if (mine.HasExploded)
                    {
                        if (IsBoxCollision(_player2.Position, _cellSize, _cellSize, mine.Position, mine.ExplosionRadius, mine.ExplosionRadius))
                        {
                            _player2.IsAlive = false;
                        }
                    }
                }
            }
        }

        public void AddMine(IMine mine)
        {
            if (mine is Mine m)
                _mines.Add(m);
        }

        public void Reset()
        {
            _walls.Clear();
            _mines.Clear();
            _prizes.Clear();
            InitializeObjects();
        }

        public void Render(Renderer renderer)
        {
            foreach (var wall in _walls) wall.Render(renderer);
            foreach (var mine in _mines) mine.Render(renderer);
            foreach (var prize in _prizes) prize.Render(renderer);
            if (_player1 != null) _player1.Render(renderer);
            if (_player2 != null) _player2.Render(renderer);
        }

        public void Dispose()
        {
            foreach (var wall in _walls) wall.Dispose();
            foreach (var mine in _mines) mine.Dispose();
            foreach (var prize in _prizes) prize.Dispose();
            _player1?.Dispose();
            _player2?.Dispose();
        }

        public void HandleKeyDown(Keys key)
        {
            _player1?.HandleKeyDown(key, this, _bombTimer, _explosionRadius);
            _player2?.HandleKeyDown(key, this, _bombTimer, _explosionRadius);
        }

        public void HandleKeyUp(Keys key)
        {
            _player1?.HandleKeyUp(key);
            _player2?.HandleKeyUp(key);
        }

        private void SpawnPrizes(float deltaTime)
        {
            _lastPrizeSpawnTime += deltaTime;
            if (_lastPrizeSpawnTime >= 5.0f)
            {
                float x = _random.Next((int)_cellSize, _windowSize.X - (int)_cellSize);
                float y = _random.Next((int)_cellSize, _windowSize.Y - (int)_cellSize);
                _prizes.Add(_prizeFactory.CreatePrize(new Vector2(x, y)));
                _lastPrizeSpawnTime = 0f;
            }
        }

        private void UpdateMines(float deltaTime)
        {
            foreach (var mine in _mines.ToList())
            {
                mine.Update(deltaTime);
                if (mine.HasExploded)
                {
                    _mines.Remove(mine);
                    mine.Dispose();
                }
            }
        }

        public IReadOnlyList<Miner> GetMiners()
        {
            return Miners;
        }
    }
}