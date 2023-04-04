using System;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8SocketConnection
    {
        
        public event Action SocketOnClosed;

        public event Action SocketOnConnected;
    }
}