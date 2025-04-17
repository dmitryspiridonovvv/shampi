using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace MinerGame.Core
{
    public class InputHandler
    {
        private readonly HashSet<Keys> _pressedKeys = new();
        private Vector2i _windowSize;

        public event Action<Keys>? OnKeyDown;
        public event Action<Keys>? OnKeyUp;
        public event Action<Vector2>? OnMouseClick;

        public void KeyDown(Keys key)
        {
            if (_pressedKeys.Add(key))
            {
                OnKeyDown?.Invoke(key);
            }
        }

        public void KeyUp(Keys key)
        {
            if (_pressedKeys.Remove(key))
            {
                OnKeyUp?.Invoke(key);
            }
        }

        public void MouseClick(Vector2 position)
        {
            OnMouseClick?.Invoke(position);
        }

        public void Resize(Vector2i windowSize)
        {
            _windowSize = windowSize;
        }
    }
}