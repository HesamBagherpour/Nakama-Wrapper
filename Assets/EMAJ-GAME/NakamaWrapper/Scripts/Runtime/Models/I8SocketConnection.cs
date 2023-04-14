using System;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8SocketConnection
    {
        
        public event Action SocketOnClosed;

        public event Action SocketOnConnected;
    }
}