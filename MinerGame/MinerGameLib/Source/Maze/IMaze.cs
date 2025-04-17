using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MinerGame.GameObjects;
using System.Collections.Generic;

namespace MinerGame.Core
{
    public interface IMaze : IRenderable, IDisposable
    {
        IReadOnlyList<Miner> Miners { get; }
        IReadOnlyList<Miner> GetMiners();
        bool IsCollision(Vector2 position, float width, float height);
        void Update(float deltaTime);
        void HandleKeyDown(Keys key);
        void HandleKeyUp(Keys key);
        void AddMine(IMine mine);
        void Reset();
    }
}