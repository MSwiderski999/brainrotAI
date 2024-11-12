namespace brAI_lib.Classes 
{
    public partial class Chess
    {
        IEnumerable<Move> filterLegalMoves(Piece[] board, int from, int to, int flags, string us)
        {
            if (board[from].type == PAWN && (rank(to) == RANK_8 || rank(to) == RANK_1))
            {
                string[] pieces = { QUEEN, ROOK, BISHOP, KNIGHT };
                for (int i = 0; i < pieces.Length; i++)
                {
                    Move move = build_move(board, from, to, flags, pieces[i]);
                    make_move(move);
                    if (!king_attacked(us)) yield return move;
                    undo_move();
                }
            }
            else
            {
                Move move = build_move(board, from, to, flags);
                make_move(move);
                bool legal = !king_attacked(us);
                undo_move();
                if (legal) yield return move;
            }
        }

        public IEnumerable<Move> GenerateLegalMovesEnumerable()
        {

            string us = turn;
            string them = swap_color(us);
            Dictionary<string, int> second_rank = new Dictionary<string, int>() { { "b", RANK_7 }, { "w", RANK_2 } };
            int first_sq = SQUARES["a8"];
            int last_sq = SQUARES["h1"];
            bool single_sqaure = false;

            for (int i = first_sq; i <= last_sq; i++)
            {
                if ((i & 0x88) != 0)
                {
                    i += 7;
                    continue;
                }

                Piece piece = board[i];
                if (piece == null) continue;
                if (piece.color != us) continue;

                if (piece.type == PAWN)
                {
                    int square;
                    /* pawn captures */
                    for (int j = 2; j < 4; j++)
                    {
                        square = i + PAWN_OFFSETS[us][j];
                        if ((square & 0x88) != 0) continue;
                        if (board[square] != null && board[square].color == them)
                            foreach (Move move in filterLegalMoves(board, i, square, BITS.CAPTURE, us)) yield return move;
                        else if (square == ep_square)
                            foreach (Move move in filterLegalMoves(board, i, ep_square, BITS.EP_CAPTURE, us)) yield return move;
                    }


                    /* single square, non-capturing */
                    square = i + PAWN_OFFSETS[us][0];
                    if (board[square] == null)
                    {
                        foreach (Move move in filterLegalMoves(board, i, square, BITS.NORMAL, us)) yield return move;
                        /* double square */
                        square = i + PAWN_OFFSETS[us][1];
                        if (second_rank[us] == rank(i) && board[square] == null)
                            foreach (Move move in filterLegalMoves(board, i, square, BITS.BIG_PAWN, us)) yield return move;
                    }                    
                }
                else
                {
                    for (int j = 0; j < PIECE_OFFSETS[piece.type].Length; j++)
                    {
                        int offset = PIECE_OFFSETS[piece.type][j];
                        int square = i;
                        while (true)
                        {
                            square += offset;
                            if ((square & 0x88) != 0) break;
                            if (board[square] != null) 
                            {
                                if (board[square].color != us)
                                    foreach (Move move in filterLegalMoves(board, i, square, BITS.CAPTURE, us)) yield return move;
                                break;
                            }
                            else
                                foreach (Move move in filterLegalMoves(board, i, square, BITS.NORMAL, us)) yield return move;

                            /* break, if knight or king */
                            if (piece.type == "n" || piece.type == "k") break;
                        }
                    }
                }

            }

            /* check for castling if: a) we're generating all moves, or b) we're doing
        * single square move generation on the king's square
        */

            if (!single_sqaure || last_sq == kings[us])
            {
                /* king-side castling */
                if ((castling[us] & BITS.KSIDE_CASTLE) != 0)
                {
                    int castling_from = kings[us];
                    int castling_to = castling_from + 2;
                    if (board[castling_from + 1] == null && board[castling_to] == null &&
                        !attacked(them, kings[us]) &&
                        !attacked(them, castling_from + 1) &&
                        !attacked(them, castling_to)
                    ) foreach (Move move in filterLegalMoves(board, kings[us], castling_to, BITS.KSIDE_CASTLE, us)) 
                        yield return move;
                }

                /* queen-side castling */
                if ((castling[us] & BITS.QSIDE_CASTLE) != 0)
                {
                    int castling_from = kings[us];
                    int castling_to = castling_from - 2;
                    if (board[castling_from - 1] == null &&
                        board[castling_from - 2] == null &&
                        board[castling_from - 3] == null &&
                        !attacked(them, kings[us]) &&
                        !attacked(them, castling_from - 1) &&
                        !attacked(them, castling_to)
                    ) foreach (Move move in filterLegalMoves(board, kings[us], castling_to, BITS.QSIDE_CASTLE, us)) 
                        yield return move;
                }
            }
        }
    }
}