using MinerGame.Core;
using OpenTK.Mathematics;
using System;

namespace MinerGame.UI
{
    public class MenuRenderer : IDisposable
    {
        private readonly Renderer _renderer;
        private readonly FontRenderer _fontRenderer;
        private readonly Vector2i _windowSize;

        public MenuRenderer(Renderer renderer, Vector2i windowSize)
        {
            _renderer = renderer;
            _windowSize = windowSize;
            _fontRenderer = new FontRenderer(renderer, "Resources/Fonts/Agitpropc.otf", 16);
        }

        public void Render(Renderer renderer)
        {
            Console.WriteLine("Rendering MenuRenderer");
            string text = "Miner Game\nPress Enter to Start\nS for Settings";
            var lines = text.Split('\n');
            float lineHeight = 20;
            float y = _windowSize.Y / 2 - (lines.Length - 1) * lineHeight / 2; // Центрирование по Y

            foreach (var line in lines)
            {
                var textSize = new Vector2(line.Length * 10, 16);
                var position = new Vector2((_windowSize.X - textSize.X) / 2, y); // Центрирование по X
                _fontRenderer.RenderText(line, position, 1.0f);
                y += lineHeight; // Сдвиг вниз
            }
        }

        public void Dispose()
        {
            _fontRenderer.Dispose();
        }
    }
}