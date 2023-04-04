using System;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class MultiPlayerMessage<T>
    {
        public string uuid;
        public string needLastStateUserId;
        public T message;
    }
}