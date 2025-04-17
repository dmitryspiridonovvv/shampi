using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Mine : GameObject
    {
        public Mine(Vector2 position) : base(position, "mine.png") { }

        protected override string GetTextureName() => "mine.png";
    }
}