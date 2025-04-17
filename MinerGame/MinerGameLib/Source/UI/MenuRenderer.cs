using System.Drawing;
using MinerGameLib.Source.UI;

namespace MinerGameLib.Source.UI
{
    public class MenuRenderer
    {
        private readonly FontRenderer _fontRenderer;

        public MenuRenderer(string fontPath, float fontSize)
        {
            _fontRenderer = new FontRenderer(fontPath, fontSize);
        }

        public Bitmap RenderMenu(string menuText)
        {
            return _fontRenderer.RenderText(menuText, Color.White, 800, 600);
        }

        public void Dispose()
        {
            _fontRenderer.Dispose();
        }
    }
}