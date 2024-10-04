using brAI_lib.Interfaces;

namespace brAI_lib.Classes
{
    public static class Evaluation
    {
        /// <summary>
        /// Values of each type of chess pieces
        /// </summary>
        private readonly static Dictionary<string, double> PieceValues = new()
        {
            {"p", 10 },
            {"k", 30 },
            {"b", 35 },
            {"r", 50 },
            {"q", 90 },
            {"k", 900 }
        };

        /// <summary>
        /// Values of each square for a pawn
        /// </summary>
        private readonly static double[,] PawnSquareValues = new double[8, 8]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 5, 5, 5, 5, 5, 5, 5, 5 },
            { 1, 1, 2, 3, 3, 2, 1, 1 },
            { 0.5, 0.5, 1, 2.5, 2.5, 1, 0.5, 0.5 },
            { 0, 0, 0, 2, 2, 0, 0, 0 },
            { 0.5, -0.5, -1, 0, 0, -1, -0.5, 0.5},
            { 0.5, 1, 1, -2, -2, 1, 1, 0.5 },
            { 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        /// <summary>
        /// Values of each square for a knight
        /// </summary>
        private readonly static double[,] KnightSquareValues = new double[8, 8]
        {
            { -5, -4, -3, -3, -3, -3, -4, -5 },
            { -4, -2, 0, 0, 0, 0, -2, -4 },
            { -3, 0, 1, 1.5, 1.5, 1, 0, -3 },
            { -3, 0.5, 1.5, 2, 2, 1.5, 0.5, -3 },
            { -3, 0, 1.5, 2, 2, 1.5, 0.5, -3 },
            { -3, 0.5, 1, 1.5, 1.5, 1, 0.5, -3 },
            { -4, -2, 0, 0.5, 0.5, 0, -2, -4 },
            { -5, -4, -3, -3, -3, -3, -4, -5 }
        };

        /// <summary>
        /// Values of each square for a bishop
        /// </summary>
        private static readonly double[,] BishopSquareValues = new double[8, 8]
        {
            { -2, -1, -1, -1, -1, -1, -1, -2 },
            { -1, 0, 0, 0, 0, 0, 0, -1 },
            { -1, 0, 0.5, 1, 1, 0.5, 0, -1 },
            { -1, 0.5, 0.5, 1, 1, 0.5, 0.5, -1 },
            { -1, 0, 1, 1, 1, 1, 0, -1 },
            { -1, 1, 1, 1, 1, 1, 1, -1 },
            { -1, 0.5, 0, 0, 0, 0, 0.5, -1 },
            { -2, -1, -1, -1, -1, -1, -1, -2 }
        };


        /// <summary>
        /// Values of each square for a rook
        /// </summary>
        private static readonly double[,] RookSquareValues = new double[8, 8]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0.5, 1, 1, 1, 1, 1, 1, 0.5 },
            { -0.5, 0, 0, 0, 0, 0, 0, -0.5 },
            { -0.5, 0, 0, 0, 0, 0, 0, -0.5 },
            { -0.5, 0, 0, 0, 0, 0, 0, -0.5 },
            { -0.5, 0, 0, 0, 0, 0, 0, -0.5 },
            { -0.5, 0, 0, 0, 0, 0, 0, -0.5 },
            { 0, 0, 0, 0.5, 0.5, 0, 0, 0 }
        };

        /// <summary>
        /// Values of each square for a queen
        /// </summary>
        private static readonly double[,] QueenSquareValues = new double[8, 8]
        {
            { -2, -1, -1, -0.5, -0.5, -1, -1, -2},
            { -1, 0, 0, 0, 0, 0, 0, -1},
            { -1, 0, 0.5, 0.5, 0.5, 0.5, 0, -1},
            { -0.5, 0, 0.5, 0.5, 0.5, 0.5, 0.5, -0.5},
            { 0, 0, 0.5, 0.5, 0.5, 0.5, 0, 0},
            { -1, 0, 0.5, 0.5, 0.5, 0.5, 0, -1},
            { -1, 0, 0, 0, 0, 0, 0, -1},
            { -2, -1, -1, -0.5, -0.5, -1, -1, -2}
        };

        /// <summary>
        /// Values of each square for a king
        /// </summary>
        private static readonly double[,] KingSquareValues = new double[8, 8]
        {
            { -3, -4, -4, -5, -5, -4, -4, -3 },
            { -3, -4, -4, -5, -5, -4, -4, -3 },
            { -3, -4, -4, -5, -5, -4, -4, -3 },
            { -3, -4, -4, -5, -5, -4, -4, -3 },
            { -2, -3, -3, -4, -4, -3, -3, -2 },
            { -1, -2, -2, -2, -2, -2, -2, -1 },
            { 2, 2, 0, 0, 0, 0, 2, 2 },
            { 2, 3, 1, 0, 0, 1, 3, 2 }
        };

        public static double Evaluate(Chess chess)
        {
            throw new NotImplementedException();
        }
    }
}
