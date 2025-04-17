using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Wall : GameObject
    {
        private static readonly string[] textures = { "wall1.png", "wall2.png", "wall3.png", "wall4.png", "wall5.png" };
        private readonly string textureName;

        public Wall(Vector2 position) : base(position, GetRandomTexture())
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