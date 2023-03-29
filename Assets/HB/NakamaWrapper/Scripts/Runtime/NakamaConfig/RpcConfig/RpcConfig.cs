using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.RpcConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New RPC Config" , fileName = "RPC config")]
    public class RpcConfig : ScriptableObject
    {

        public string roomToken;
        public MatchLabelFilter roomTokenPayload;
        public string rpcName;
        public int timeOutSec;
        public int maxRetries;
        public int baseDelayMs;
    }
}