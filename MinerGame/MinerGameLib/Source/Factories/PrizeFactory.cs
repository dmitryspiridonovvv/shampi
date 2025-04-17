using OpenTK.Mathematics;
using MinerGame.GameObjects;
using MinerGame.Core;

namespace MinerGame.Maze
{
    public class PrizeFactory
    {
        private readonly float _cellSize;
        private readonly Renderer _renderer;
        private readonly Random _random = new();

        public PrizeFactory(float cellSize, Renderer renderer)
        {
            _cellSize = cellSize;
            _renderer = renderer;
        }

        public Prize CreatePrize(Vector2 position)
        {
            int effect = _random.Next(1, 4); // 1: speed, 2: power, 3: trap
            if (effect == 3) effect = -1;
            return new Prize(position, "Resources/Textures/prize.png", _cellSize, effect, _renderer);
        }
    }
}