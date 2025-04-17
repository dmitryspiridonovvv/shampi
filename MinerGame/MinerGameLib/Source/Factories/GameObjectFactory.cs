using OpenTK.Mathematics;
using MinerGame.Core;
using MinerGame.GameObjects;

namespace MinerGame.Factories
{
    public class GameObjectFactory
    {
        private readonly Renderer _renderer;

        public GameObjectFactory(Renderer renderer)
        {
            _renderer = renderer;
        }

        public Wall CreateWall(Vector2 position, float cellSize)
        {
            return new Wall(position, "Resources/Textures/wall.png", cellSize, _renderer);
        }

        public Miner CreateMiner(Vector2 position, bool isPlayer1, float cellSize, float speed, float mineCooldown)
        {
            return new Miner(position, isPlayer1 ? "Resources/Textures/player1.png" : "Resources/Textures/player2.png", isPlayer1, 1, cellSize, speed, _renderer, mineCooldown);
        }

        public Mine CreateMine(Vector2 position, float cellSize, int explosionPower, float explosionRadius, float bombTimer)
        {
            return new Mine(position, "Resources/Textures/mine.png", cellSize, explosionPower, explosionRadius, bombTimer, _renderer);
        }

        public Prize CreatePrize(Vector2 position, float cellSize, int effect)
        {
            return new Prize(position, "Resources/Textures/prize.png", cellSize, effect, _renderer);
        }
    }
}