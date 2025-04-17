using OpenTK.Mathematics;
using MinerGame.Core;
using OpenTK.Graphics.OpenGL4;

namespace MinerGame.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        private readonly TextureManager _textureManager;
        protected readonly Renderer _renderer;
        private int _textureId;
        private readonly float _cellSize;
        private bool _isDisposed;

        public Vector2 Position { get; set; }

        protected GameObject(Vector2 position, string texturePath, float cellSize, Renderer renderer)
        {
            Position = position;
            _cellSize = cellSize;
            _renderer = renderer;
            _textureManager = renderer.TextureManager;
            _textureId = _textureManager.LoadTexture(texturePath);
        }

        public virtual void Render(Renderer renderer)
        {
            if (_isDisposed) return;

            var texCoords = new Vector4(0, 0, 1, 1);
            renderer.DrawSprite(_textureId, Position, new Vector2(_cellSize, _cellSize), texCoords);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _textureManager.UnloadTexture(_textureId);
                _isDisposed = true;
            }
        }
    }

    public interface IGameObject : IRenderable, IDisposable
    {
        Vector2 Position { get; }
    }
}