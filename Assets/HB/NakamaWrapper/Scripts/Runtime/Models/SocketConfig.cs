namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class SocketConfig
    {
        public readonly bool AppearOnline;
        public readonly int ConnectionTimeout;
        public readonly int PingPongIntervalSec;
        public readonly int NetworkCheckIntervalSec;
        public SocketConfig()
        {
            PingPongIntervalSec =3;
            AppearOnline = false;
            ConnectionTimeout = 20;
            NetworkCheckIntervalSec = 9;
        }
    }
}