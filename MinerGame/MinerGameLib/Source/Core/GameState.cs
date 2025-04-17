namespace MinerGame.Core
{
    public abstract class GameState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Render(Renderer renderer) { }
    }
}