using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.UI
{
    public class UIComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int TextureId { get; set; }
        public bool IsHovered { get; set; }

        public UIComponent(Vector2 position, Vector2 size, int textureId)
        {
            Position = position;
            Size = size;
            TextureId = textureId;
        }

        public bool Contains(Vector2 point)
        {
            return point.X >= Position.X && point.X <= Position.X + Size.X &&
                   point.Y >= Position.Y && point.Y <= Position.Y + Size.Y;
        }

        public void Render(Renderer renderer)
        {
            var texCoords = new Vector4(0, 0, 1, 1);
            renderer.DrawSprite(TextureId, Position, Size, texCoords);
        }
    }
}