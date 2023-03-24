using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig
{
    [CreateAssetMenu( menuName = "HB/Nakama/Create New Client Config" , fileName = "Client Config")]
    public class ServerClientConfigs : ScriptableObject
    {
        public string scheme = "http";
        public string host = "localhost";
        public int port = 7350;
        public string serverKey = "defaultkey";
        public bool autoRefreshSession = true;
    }
}