using brAI_lib.Classes;

namespace brAI_tests
{
    public class ChessTests
    {
        [Fact]
        public void TestChess()
        {
            Chess chess = new Chess();

            Assert.NotNull(chess);
        }

        [Fact]
        public void TestChessLoadFen()
        {
            string fen = "r1bqkb1r/1ppp1ppp/p1n2n2/4p3/B3P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1";
            Chess chess = new Chess(fen);

            Assert.NotNull(chess);
        }

        [Fact]
        public void TestChessLoadFenException()
        {
            string fen = "invalid fen";
            Chess chess;
            Assert.Throws<ArgumentException>(() => chess = new(fen));
        }

        [Fact]
        public void TestAscii()
        {
            Chess chess = new Chess();
            string ascii = chess.Ascii();

            Assert.False(ascii == string.Empty);
        }

        [Fact]
        public void TestClearTrue()
        {
            Chess chess = new Chess();
            chess.Clear();

            bool clear = true;
            foreach (string square in Chess.SQUARES.Keys)
            {
                if (chess.GetPiece(square) != null)
                {
                    clear = false;
                    break;
                }
            }

            Assert.True(clear);
        }
    }
}
