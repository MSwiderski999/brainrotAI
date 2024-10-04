using brAI_lib.Classes;

namespace brAI_lib.Interfaces
{
    public interface IBot
    {
        string FindBestMove(Chess chess, int depth, int timeoutMs = -1);
    }
}
