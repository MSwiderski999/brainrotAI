using brAI_lib.Classes;

namespace brAI_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chess chess = new();

            Console.WriteLine(chess.Ascii());

            double eval = Evaluation.Evaluate(chess);
            Console.WriteLine($"Evaluation: {eval:f2}");
        }
    }
}
