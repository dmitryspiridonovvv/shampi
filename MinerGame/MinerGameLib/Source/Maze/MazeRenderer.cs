using MinerGame.Core;

namespace MinerGame.Maze
{
    public class MazeRenderer : IRenderable
    {
        private readonly IMaze _maze;

        public MazeRenderer(IMaze maze)
        {
            _maze = maze;
        }

        public void Render(Renderer renderer)
        {
            _maze.Render(renderer);
        }
    }
}