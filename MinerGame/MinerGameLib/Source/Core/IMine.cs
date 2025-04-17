using OpenTK.Mathematics;

namespace MinerGame.Core
{
    public interface IMine : IRenderable, IDisposable
    {
        Vector2 Position { get; }
        int ExplosionPower { get; }
        float ExplosionRadius { get; }
        bool HasExploded { get; }
        void Update(float deltaTime); // Добавлено
    }
}