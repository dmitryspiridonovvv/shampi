using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class PrizeFactory
    {
        private readonly Maze maze;
        private readonly Random random;

        public PrizeFactory(Maze maze)
        {
            this.maze = maze;
            random = new Random();
        }

        public void GeneratePrizes(List<GameObject> gameObjects)
        {
            int prizeCount = 5;
            int cellSize = 32;
            int mapHeight = 22; // Высота карты в клетках (из maze.txt)

            for (int i = 0; i < prizeCount; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, 39);
                    y = random.Next(1, 21);
                } while (maze.IsWallAt(x, y));

                // Инвертируем Y: (mapHeight - 1 - y) * cellSize
                gameObjects.Add(new Prize(new Vector2(x * cellSize, (mapHeight - 1 - y) * cellSize)));
            }
        }
    }
}