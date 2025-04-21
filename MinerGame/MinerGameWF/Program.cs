using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.IO;
using System.Text.Json;

namespace MinerGameWF
{
    class Program
    {
        private static readonly string[] ConfigPaths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gameconfig.json"),
            @"W:\education\Курсовая\MinerGame\MinerGameLib\Config\Gameconfig.json"
        };

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("[Program.Main] Application started.");

                // Загрузка конфигурации
                string configPath = null;
                foreach (var path in ConfigPaths)
                {
                    Console.WriteLine($"[Program.Main] Checking Gameconfig.json at: {path}");
                    if (File.Exists(path))
                    {
                        configPath = path;
                        break;
                    }
                }

                if (configPath == null)
                {
                    Console.WriteLine("[Program.Main] Error: Gameconfig.json not found at any of the following paths:");
                    foreach (var path in ConfigPaths)
                        Console.WriteLine($"  - {path}");
                    Console.WriteLine("[Program.Main] Please ensure Gameconfig.json exists in W:\\education\\Курсовая\\MinerGame\\MinerGameLib\\Config and is copied to W:\\education\\Курсовая\\MinerGame\\MinerGameWF\\bin\\Debug\\net8.0.");
                    return;
                }

                Console.WriteLine($"[Program.Main] Loading Gameconfig.json from: {configPath}");
                string configJson;
                try
                {
                    configJson = File.ReadAllText(configPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Program.Main] Error reading Gameconfig.json: {ex.Message}");
                    Console.WriteLine($"[Program.Main] Stack trace: {ex.StackTrace}");
                    return;
                }

                Console.WriteLine("[Program.Main] Deserializing Gameconfig.json...");
                GameConfig config;
                try
                {
                    config = JsonSerializer.Deserialize<GameConfig>(configJson) ?? throw new InvalidOperationException("Deserialization returned null.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Program.Main] Error deserializing Gameconfig.json: {ex.Message}");
                    Console.WriteLine($"[Program.Main] Stack trace: {ex.StackTrace}");
                    Console.WriteLine("[Program.Main] Please ensure Gameconfig.json has the correct format (e.g., valid JSON with DefaultResolution, PlayerSpeed, etc.).");
                    return;
                }

                var windowSettings = new NativeWindowSettings
                {
                    ClientSize = new Vector2i(config.DefaultResolution!.X, config.DefaultResolution.Y),
                    Title = "Miner Game"
                };

                Console.WriteLine("[Program.Main] Creating GameWindow...");
                using (var game = new GameWindow(GameWindowSettings.Default, windowSettings))
                {
                    Console.WriteLine("[Program.Main] Starting game...");
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Program.Main] Fatal error: {ex.Message}");
                Console.WriteLine($"[Program.Main] Stack trace: {ex.StackTrace}");
            }
        }
    }

    public class GameConfig
    {
        public Resolution? DefaultResolution { get; set; }
        public float PlayerSpeed { get; set; }
        public float BombTimer { get; set; }
        public float PrizeSpawnInterval { get; set; }
        public float MineCooldown { get; set; }
        public float ExplosionRadius { get; set; }
    }

    public class Resolution
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}