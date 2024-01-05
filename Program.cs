using System.Reflection.Metadata.Ecma335;

namespace _2048_CON
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to 2048-CON!");
            Console.WriteLine("Use the arrow keys to move the tiles or press 'Q' to quit.");
            Console.WriteLine();

            Game g = new Game();

            while (true)
            {
                g.InitGame();

                while (!g.GameOver())
                {
                    Console.Clear();
                    g.RenderGame();

                    var key = Console.ReadKey().Key;

                    if (key == ConsoleKey.Q)
                        return;

                    g.MoveTiles(key);
                }

                Console.Clear();
                g.RenderGame();

                Console.WriteLine("Game Over! Press 'R' to restart or press 'Q' to quit.");

                while (true)
                {
                    var key = Console.ReadKey().Key;

                    if (key == ConsoleKey.R)
                        break;

                    if (key == ConsoleKey.Q)
                        return;
                }
            }
        }
    }
}
