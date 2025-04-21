using MinerGame.Core;
using MinerGame.GameObjects;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;

namespace MinerGame.Maze
{
    public interface IMaze
    {
        IReadOnlyList<Miner> Miners { get; }
        void Update(float deltaTime);
        bool IsCollision(Vector2 position, float width, float height);
        void AddMine(IMine mine);
        void Reset();
        void Render(Renderer? renderer);
        void Dispose();
        void HandleKeyDown(Keys key);
        void HandleKeyUp(Keys key);
        IReadOnlyList<Miner> GetMiners();
    }
}