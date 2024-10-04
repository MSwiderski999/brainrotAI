using brAI_lib.Interfaces;

namespace brAI_lib.Classes
{
    public static class Evaluation
    {
        /// <summary>
        /// Values of each type of chess pieces.
        /// </summary>
        private readonly static Dictionary<string, double> PieceValues = new()
        {
            {"p", 10 },
            {"n", 30 },
            {"b", 35 },
            {"r", 50 },
            {"q", 90 },
            {"k", 0 }
        };

        /// <summary>
        /// Values of each square for a pawn.
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
        /// Values of each square for a knight.
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
        /// Values of each square for a bishop.
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
        /// Values of each square for a rook.
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
        /// Values of each square for a queen.
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
        /// Values of each square for a king.
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

        /// <summary>
        /// Gives an evaluation of a chess position.
        /// Positive better for white, negative better for black.
        /// </summary>
        /// <param name="chess">Chess object</param>
        /// <returns>Position evaluation.</returns>
        public static double Evaluate(Chess chess)
        {
            return GetMaterial(chess);
        }

        /// <summary>
        /// Gets the material value of a piece.
        /// </summary>
        /// <param name="piece">Chess piece</param>
        /// <returns>Value of the piece, negative is the piece is black, positive if white.</returns>
        private static double GetPieceValue(Piece piece)
        {
            return piece.color == "w" ? PieceValues[piece.type] : PieceValues[piece.type] * -1;
        }

        /// <summary>
        /// Counts all material in a given position.
        /// </summary>
        /// <param name="chess">Chess object</param>
        /// <returns>Material balance between white and black</returns>
        private static double GetMaterial(Chess chess)
        {
            double material = 0;

            foreach (string square in Chess.SQUARES.Keys)
            {
                Piece piece = chess.GetPiece(square);

                if (piece != null)
                {
                    material += GetPieceValue(piece);
                }
            }

            return material;
        }
    }
}
