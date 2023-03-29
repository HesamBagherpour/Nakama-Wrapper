using System;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8SocketConnection
    {
        
        public event Action SocketOnClosed;

        public event Action SocketOnConnected;
    }
}