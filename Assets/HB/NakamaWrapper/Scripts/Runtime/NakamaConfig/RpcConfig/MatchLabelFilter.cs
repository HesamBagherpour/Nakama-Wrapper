using Newtonsoft.Json;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Editor.NakamaConfig.RpcConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New MatchLabelFilter  Config" , fileName = "MatchLabelFilter config")]
    public class MatchLabelFilter : ScriptableObject
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}