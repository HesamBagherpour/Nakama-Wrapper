using Newtonsoft.Json;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.RpcConfig
{
    [CreateAssetMenu( menuName = "HB/Nakama/Create New MatchLabelFilter  Config" , fileName = "MatchLabelFilter config")]
    public class MatchLabelFilter : ScriptableObject
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}