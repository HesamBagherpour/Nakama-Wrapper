using System;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class RpcConfig
    {
        
        public RpcConfig(string roomToken, MatchLabelFilter roomTokenPayload, string rpcName, int timeOutSec, int maxRetries, int baseDelayMs)
        {
            this.roomToken = roomToken;
            RoomTokenPayload = roomTokenPayload;
            this.rpcName = rpcName;
            this.timeOutSec = timeOutSec;
            this.maxRetries = maxRetries;
            this.baseDelayMs = baseDelayMs;
        }
        public RpcConfig()
        {
            this.roomToken = "generalRoomToken";
            RoomTokenPayload = new MatchLabelFilter();
            this.rpcName = "JoinOrCreateMatchRpc";            
            this.timeOutSec = 20;      
            this.maxRetries = 3;      
            this.baseDelayMs = 100;    
        }
        public string roomToken;
        public MatchLabelFilter RoomTokenPayload;
        public string rpcName;
        public int timeOutSec;
        public int maxRetries;
        public int baseDelayMs;

    }
}