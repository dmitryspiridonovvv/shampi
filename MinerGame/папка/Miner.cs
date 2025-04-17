using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace MinerGameLib
{
    public class Miner : GameObject
    {
        private readonly bool isFirstPlayer;
        private Vector2 velocity;
        private float speed = 100.0f;
        private int score;
        private float mineCooldown = 3.0f; // Задержка в секундах
        private float lastMineTime;

        public Miner(Vector2 position, string textureName, bool isFirstPlayer) : base(position, textureName)
        {
            this.isFirstPlayer = isFirstPlayer;
            lastMineTime = -mineCooldown; // Чтобы можно было сразу заложить мину
        }

        public override void Update(float deltaTime, List<GameObject> gameObjects)
        {
            if (!IsActive) return;

            Vector2 newPosition = Position + velocity * deltaTime;
            bool canMove = true;

            foreach (var obj in gameObjects)
            {
                if (obj == this || !obj.IsActive) continue;

                if (obj is Wall && IsColliding(newPosition, obj))
                {
                    canMove = false;
                    break;
                }
                if (obj is Mine && IsColliding(newPosition, obj))
                {
                    IsActive = false;
                    return;
                }
                if (obj is Prize prize && IsColliding(newPosition, obj))
                {
                    score += prize.Value;
                    prize.IsActive = false;
                }
            }

            if (canMove)
                Position = newPosition;
        }

        private bool IsColliding(Vector2 newPosition, GameObject other)
        {
            return newPosition.X < other.Position.X + 32 &&
                   newPosition.X + 32 > other.Position.X &&
                   newPosition.Y < other.Position.Y + 32 &&
                   newPosition.Y + 32 > other.Position.Y;
        }

        public void HandleKeyDown(Keys key, List<GameObject> gameObjects)
        {
            if (isFirstPlayer)
            {
                if (key == Keys.W) velocity.Y = -speed;
                if (key == Keys.S) velocity.Y = speed;
                if (key == Keys.A) velocity.X = -speed;
                if (key == Keys.D) velocity.X = speed;
                if (key == Keys.Space && CanPlaceMine())
                {
                    PlaceMine(gameObjects);
                    lastMineTime = (float)DateTime.Now.TimeOfDay.TotalSeconds;
                }
            }
            else
            {
                if (key == Keys.Up) velocity.Y = -speed;
                if (key == Keys.Down) velocity.Y = speed;
                if (key == Keys.Left) velocity.X = -speed;
                if (key == Keys.Right) velocity.X = speed;
                if (key == Keys.Enter && CanPlaceMine())
                {
                    PlaceMine(gameObjects);
                    lastMineTime = (float)DateTime.Now.TimeOfDay.TotalSeconds;
                }
            }
        }

        private bool CanPlaceMine()
        {
            float currentTime = (float)DateTime.Now.TimeOfDay.TotalSeconds;
            return currentTime - lastMineTime >= mineCooldown;
        }

        private void PlaceMine(List<GameObject> gameObjects)
        {
            gameObjects.Add(new Mine(Position));
        }

        public void HandleKeyUp(Keys key)
        {
            if (isFirstPlayer)
            {
                if (key == Keys.W && velocity.Y < 0) velocity.Y = 0;
                if (key == Keys.S && velocity.Y > 0) velocity.Y = 0;
                if (key == Keys.A && velocity.X < 0) velocity.X = 0;
                if (key == Keys.D && velocity.X > 0) velocity.X = 0;
            }
            else
            {
                if (key == Keys.Up && velocity.Y < 0) velocity.Y = 0;
                if (key == Keys.Down && velocity.Y > 0) velocity.Y = 0;
                if (key == Keys.Left && velocity.X < 0) velocity.X = 0;
                if (key == Keys.Right && velocity.X > 0) velocity.X = 0;
            }
        }

        protected override string GetTextureName() => isFirstPlayer ? "miner1.png" : "miner2.png";
    }
}