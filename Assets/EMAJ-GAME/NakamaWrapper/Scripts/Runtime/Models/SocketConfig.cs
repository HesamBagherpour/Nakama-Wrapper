namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class SocketConfig
    {
        public readonly bool AppearOnline;
        public readonly int ConnectionTimeout;

        public readonly bool matchMessageController;

        public SocketConfig()
        {
            AppearOnline = false;
            ConnectionTimeout = 20;
        }
    }

    
    
}