using System;

namespace DebugReloaded.Interface {
    /// <summary>
    /// Scrive sulla console con colori
    /// </summary>
    public static class ConsoleLogger {
        public static void Write(object text, string type, ConsoleColor color = ConsoleColor.White) {
            ConsoleColor oldcolor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.WriteLine($"{DateTime.Now:T} [{type}] {text}");

            Console.ForegroundColor = oldcolor;
        }
    }
}