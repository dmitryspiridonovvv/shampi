using OpenTK.Windowing.Desktop;

namespace MinerGameWF
{
    class Program
    {
        static void Main(string[] args)
        {
            var windowSettings = new NativeWindowSettings
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "Miner Game"
            };

            using (var game = new GameWindow(GameWindowSettings.Default, windowSettings))
            {
                game.Run();
            }
        }
    }
}