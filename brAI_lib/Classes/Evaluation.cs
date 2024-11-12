using brAI_lib.Interfaces;
using System.Diagnostics;

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
            {"k", 1000 }
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
            double eval = 0;

            foreach (string square in Chess.SQUARES.Keys)
            {
                eval += GetPieceValue(chess, square);
            }
            return eval;
        }

        /// <summary>
        /// Gets the material value of a piece.
        /// </summary>
        /// <param name="piece">Chess piece</param>
        /// <returns>Value of the piece, negative is the piece is black, positive if white.</returns>
        private static double GetPieceMaterial(Piece piece)
        {
            return PieceValues[piece.type];
        }

        /// <summary>
        /// Gets the value of a piece in a given square
        /// </summary>
        /// <param name="square">Square</param>
        /// <param name="piece">Piece</param>
        /// <returns>Value of a piece towards the evaluation</returns>
        /// <exception cref="ArgumentOutOfRangeException">Square is out of bounds</exception>
        private static double GetSquareValue(string square, Piece piece)
        {
            (int, int) idx = Chess.SquareToIdx(square);

            if (piece.color == "b")
            {
                idx.Item1 = 7 - idx.Item1;
                idx.Item2 = 7 - idx.Item2;
            }

            double val = piece.type switch
            {
                "p" => PawnSquareValues[idx.Item1, idx.Item2],
                "n" => KnightSquareValues[idx.Item1, idx.Item2],
                "b" => BishopSquareValues[idx.Item1, idx.Item2],
                "r" => RookSquareValues[idx.Item1, idx.Item2],
                "q" => QueenSquareValues[idx.Item1, idx.Item2],
                "k" => KingSquareValues[idx.Item1, idx.Item2],
                _ => throw new ArgumentOutOfRangeException(nameof(piece))
            };

            return val;
        }

        /// <summary>
        /// Get value of each individual piece on the board
        /// </summary>
        /// <param name="chess">Chess object</param>
        /// <param name="square">Square to evaluate</param>
        /// <returns></returns>
        private static double GetPieceValue(Chess chess, string square)
        {
            Piece piece = chess.GetPiece(square);

            if ( piece == null ) { return 0; }

            double material = GetPieceMaterial(piece);
            double mobility = chess.LegalMovesSquare(square).Length * material / 100;
            double squareValue = GetSquareValue(square, piece);

            double val = material + mobility + squareValue;

            return piece.color == "w" ? val : -val;
        }
    }
}
