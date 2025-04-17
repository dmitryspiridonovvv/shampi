using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.GameObjects
{
    public class Mine : GameObject, IMine
    {
        private float _timer;
        private readonly float _bombTimer;

        public int ExplosionPower { get; }
        public float ExplosionRadius { get; }
        public bool HasExploded { get; private set; }

        public Mine(Vector2 position, string texturePath, float cellSize, int explosionPower, float explosionRadius, float bombTimer, Renderer renderer)
            : base(position, texturePath, cellSize, renderer)
        {
            ExplosionPower = explosionPower;
            ExplosionRadius = explosionRadius;
            _bombTimer = bombTimer;
            _timer = 0f;
            HasExploded = false;
        }

        public void Update(float deltaTime)
        {
            if (HasExploded) return;

            _timer += deltaTime;
            if (_timer >= _bombTimer)
            {
                HasExploded = true;
            }
        }
    }
}