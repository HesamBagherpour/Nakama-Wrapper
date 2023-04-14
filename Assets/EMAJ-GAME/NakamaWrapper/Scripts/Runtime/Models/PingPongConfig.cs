namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class PingPongConfig
    {
        public int PingPongIntervalSec;
        public int NetworkCheckIntervalSec;
        public long LastReceivedGameState;
        public PingPongState PingPongState;

        public PingPongConfig()
        {
        }

        public PingPongConfig(int pingPongIntervalSec, int networkCheckIntervalSec, long lastReceivedGameState, PingPongState pingPongState)
        {
            PingPongIntervalSec = pingPongIntervalSec;
            NetworkCheckIntervalSec = networkCheckIntervalSec;
            LastReceivedGameState = lastReceivedGameState;
            PingPongState = pingPongState;
        }
    }
}