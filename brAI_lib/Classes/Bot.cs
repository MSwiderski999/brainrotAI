using brAI_lib.Interfaces;

namespace brAI_lib.Classes
{
    public class Bot : IBot
    {
        /// <summary>
        /// Adversarial search used to find optimal move given the present gamestate, utilizing negamax with alpha-beta pruning.
        /// </summary>
        /// <param name="chess">State of the chess game to analize from</param>
        /// <param name="depth">Maximum depth to search until</param>
        /// <param name="timeoutMs">Maximum time to run search for (not implemented)</param>
        /// <returns>Best found move, in algebraic chess notation</returns>
        /// <exception cref="ArgumentException">Thrown if provided chess game has already reached an endstate</exception>
        public string FindBestMove(Chess chess, int depth, int timeoutMs = -1)
        {
            if (chess.GameOver()) throw new ArgumentException("Cannot perform search on concluded game");

            double value = double.NegativeInfinity;
            int color = chess.Turn() == "w" ? 1 : -1;
            string bestMove = "";  

            foreach (Move move in chess.GenerateLegalMovesEnumerable())
            {
                chess.Move(move);
                double moveValue = -alphabetaNegamax(chess, depth - 1, -color, double.NegativeInfinity, -value);
                chess.Undo();

                if (moveValue > value) 
                {
                    bestMove = chess.MoveToSan(move);
                    value = moveValue;
                }
            }

            return bestMove;
        }

        /// <summary>
        /// Implementation of negamax (variant of minimax algorithm) utilizing alpha-beta pruning.
        /// Recursively searches every possible move branch up to given depth,
        /// for every evaluation depth, picks the preffered move for the current player.
        /// Keeps track of alpha and beta, worst case scenarios for each player upon which to evaluate whether to continue checking moves from posistion.
        /// </summary>
        /// <param name="chess">State of the chess game to analize from</param>
        /// <param name="depth">Maximum depth to search until</param>
        /// <param name="color">Color of the current player (+1 for white, -1 for black)</param>
        /// <param name="alpha">Minimal maximized value guaranteed for the current player</param>
        /// <param name="beta">Minimal maximized value guaranteed for the opponent player</param>
        /// <returns>Minimaxed value of move for current depth</returns>
        private double alphabetaNegamax(Chess chess, int depth, int color, double alpha, double beta) {
            if (depth == 0 || chess.GameOver()) 
                return color * Evaluation.Evaluate(chess);
            else 
            {

                double value = double.NegativeInfinity;

                foreach (Move move in chess.GenerateLegalMovesEnumerable())
                {
                    chess.Move(move);
                    value = Math.Max(value, -alphabetaNegamax(chess, depth - 1, -color, -beta, -alpha));
                    chess.Undo();

                    alpha = Math.Max(alpha, value);
                    if (value >= beta) break;
                }
                return value;
            }
        }
    }
}
