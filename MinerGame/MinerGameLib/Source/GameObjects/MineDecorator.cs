using OpenTK.Mathematics;
using MinerGame.Core;

namespace MinerGame.GameObjects
{
    public abstract class MineDecorator : IMine
    {
        protected readonly IMine _mine;

        protected MineDecorator(IMine mine)
        {
            _mine = mine;
        }

        public Vector2 Position => _mine.Position;
        public virtual int ExplosionPower => _mine.ExplosionPower;
        public virtual float ExplosionRadius => _mine.ExplosionRadius;
        public bool HasExploded => _mine.HasExploded;

        public virtual void Update(float deltaTime) => _mine.Update(deltaTime);
        public virtual void Render(Renderer renderer) => _mine.Render(renderer);
        public virtual void Dispose() => _mine.Dispose();
    }

    public class PoweredMineDecorator : MineDecorator
    {
        private readonly int _powerIncrease;

        public PoweredMineDecorator(IMine mine, int powerIncrease) : base(mine)
        {
            _powerIncrease = powerIncrease;
        }

        public override int ExplosionPower => _mine.ExplosionPower + _powerIncrease;
    }
}