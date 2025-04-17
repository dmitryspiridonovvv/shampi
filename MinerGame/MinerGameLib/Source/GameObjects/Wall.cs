using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.GameObjects
{
    public class Wall : GameObject
    {
        public Wall(Vector2 position, string texturePath, float cellSize, Renderer renderer)
            : base(position, texturePath, cellSize, renderer)
        {
        }
    }
}