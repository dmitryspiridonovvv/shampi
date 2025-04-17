using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.GameObjects
{
    public class Prize : GameObject
    {
        public int Effect { get; }

        public Prize(Vector2 position, string texturePath, float cellSize, int effect, Renderer renderer)
            : base(position, texturePath, cellSize, renderer)
        {
            Effect = effect;
        }
    }
}