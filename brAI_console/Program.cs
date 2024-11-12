using System.Diagnostics;
using brAI_lib.Classes;

namespace brAI_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) 
            {
                Console.WriteLine("Error: insufficient arguments\nUsage: brAI_console [depth] [?fen]");
                Environment.Exit(1);
            }

            if (!int.TryParse(args[0], out int depth))
            {
                Console.WriteLine("Error: incorrent number format in argument [depth]");
                Environment.Exit(1);
            }

            Chess chess;
            try
            {
                if (args.Length > 1) chess = new(args[1]);
                else chess = new();

                Console.WriteLine("Starting position: \n" + chess.Ascii());

                Bot bot = new();
                Stopwatch sw = new();
                sw.Start();
                string best = bot.FindBestMove(chess, depth);
                sw.Stop();

                Console.WriteLine($"Best move: {best} (took {sw.ElapsedMilliseconds / 1000.0}s)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                // Console.WriteLine("Error: incorrect fen format in argument [?fen]");
                Environment.Exit(1);
            }
        }
    }
}
