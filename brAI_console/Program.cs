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
                Console.WriteLine("Error: insufficient arguments\nUsage: brAI_console [depth]");
                Environment.Exit(1);
            }

            try 
            {
                int depth = Convert.ToInt32(args[0]);
                Chess chess = new();
                Console.WriteLine("Starting position: \n" + chess.Ascii());

                Bot bot = new();
                Stopwatch sw = new();
                sw.Start();
                string best = bot.FindBestMove(chess, depth);
                sw.Stop();

                Console.WriteLine($"Best move: {best} (took {sw.ElapsedMilliseconds / 1000.0}s)");
            } 
            catch  
            {
                Console.WriteLine("Error: incorrect number format for argument [depth]");
                Environment.Exit(1);
            }
        }
    }
}
