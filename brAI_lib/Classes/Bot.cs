using brAI_lib.Interfaces;

namespace brAI_lib.Classes
{
    public class Bot : IBot
    {
        /// <summary>
        /// Adversarial search used to find optimal move given the present gamestate, utilizing negamax.
        /// </summary>
        /// <param name="chess">State of the chess game to analize from</param>
        /// <param name="depth">Maximum depth to search until</param>
        /// <param name="timeoutMs">Maximum time to run search for (not implemented)</param>
        /// <returns>Best found move, in algebraic chess notation</returns>
        /// <exception cref="ArgumentException">Thrown if provided chess game has already reached an endstate</exception>
        public string FindBestMove(Chess chess, int depth, int timeoutMs = -1)
        {
            if (chess.GameOver()) throw new ArgumentException("Cannot perform search on concluded game");

            string[] moves = chess.LegalMovesAll();
            int color = chess.Turn() == "w" ? 1 : -1;

            var moveValues = new Dictionary<string, double>();
            foreach (string move in moves) 
            {
                chess.Move(move);
                moveValues.Add(move, -negamax(chess, depth - 1, -color));
                chess.Undo();
            }

            (string, double) bestMove = ("", double.NegativeInfinity);
            foreach (var move in moveValues) 
                if (move.Value > bestMove.Item2) bestMove = (move.Key, move.Value);

            return bestMove.Item1;
        }

        /// <summary>
        /// Naïve implementation of negamax (variant of minimax algorithm).
        /// Recursively searches every possible move branch up to given depth,
        /// for every evaluation depth, picks the preffered move for the current player.
        /// </summary>
        /// <param name="chess">State of the chess game to analize from</param>
        /// <param name="depth">Maximum depth to search until</param>
        /// <param name="color">Color of the current player (+1 for white, -1 for black)</param>
        /// <returns>Minimaxed value of move for current depth</returns>
        private double negamax(Chess chess, int depth, int color) {
            if (depth == 0 || chess.GameOver()) 
                return color * Evaluation.Evaluate(chess);
            else 
            {
                double value = double.NegativeInfinity;
                string[] moves = chess.LegalMovesAll();
                foreach (string move in moves)
                {
                    chess.Move(move);
                    value = Math.Max(value, -negamax(chess, depth - 1, -color));
                    chess.Undo();
                }
                return value;
            }
        }
    }
}
