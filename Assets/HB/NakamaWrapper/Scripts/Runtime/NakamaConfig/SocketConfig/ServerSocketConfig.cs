using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.SocketConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New Socket Config" , fileName = "Socket config")]
    public class ServerSocketConfigs : ScriptableObject
    {
        public bool appearOnline;
        public int connectionTimeout;
        public int pingPongIntervalSec;
        public int networkCheckIntervalSec;
    }

}