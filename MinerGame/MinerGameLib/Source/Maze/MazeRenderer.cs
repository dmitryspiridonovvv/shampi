using MinerGame.Core;
using OpenTK.Mathematics;

namespace MinerGame.Maze
{
    public class MazeRenderer
    {
        public readonly IMaze _maze;

        public MazeRenderer(IMaze maze)
        {
            _maze = maze;
        }

        public void Render(IMaze maze)
        {
            maze.Render(null!); // Предполагается, что Maze использует внутренний Renderer
        }
    }
}