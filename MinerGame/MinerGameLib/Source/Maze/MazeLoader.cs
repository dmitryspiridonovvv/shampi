using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.Maze
{
    public class MazeLoader
    {
        public IMaze LoadRandomMaze(Vector2i windowSize, float playerSpeed, float bombTimer, float explosionRadius, float mineCooldown, Renderer renderer)
        {
            return new Maze(windowSize, playerSpeed, bombTimer, explosionRadius, mineCooldown, renderer);
        }
    }
}