using System.Collections.Generic;
using System.IO;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Maze
    {
        public List<Wall> Walls { get; } = new List<Wall>();
        private readonly bool[,] wallMap;
        private readonly int width;
        private readonly int height;

        public Maze(string fileName, int windowWidth, int windowHeight)
        {
            var lines = File.ReadAllLines(fileName);
            height = lines.Length;

            width = 0;
            foreach (var line in lines)
            {
                if (line.Length > width)
                    width = line.Length;
            }

            wallMap = new bool[width, height];
            int cellSize = 32;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < lines[y].Length && lines[y][x] == '1')
                    {
                        // Инвертируем Y: (height - 1 - y) * cellSize
                        Walls.Add(new Wall(new Vector2(x * cellSize, (height - 1 - y) * cellSize)));
                        wallMap[x, y] = true;
                    }
                }
            }
        }

        public bool IsWallAt(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return true;
            return wallMap[x, y];
        }
    }
}