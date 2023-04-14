using System;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class MultiPlayerMessage<T>
    {
        public string uuid;
        public string needLastStateUserId;
        public T message;
    }
}