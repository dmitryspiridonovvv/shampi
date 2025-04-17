using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Prize : GameObject
    {
        private static readonly string[] textures = { "prize1.png", "prize2.png", "prize3.png" };
        private readonly string textureName;

        public int Value { get; } = new System.Random().Next(1, 11);

        public Prize(Vector2 position) : base(position, GetRandomTexture())
        {
            textureName = GetRandomTexture();
        }

        private static string GetRandomTexture()
        {
            return textures[new System.Random().Next(textures.Length)];
        }

        protected override string GetTextureName() => textureName;
    }
}