using Newtonsoft.Json;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.NakamaConfig.RpcConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New MatchLabelFilter  Config" , fileName = "MatchLabelFilter config")]
    public class MatchLabelFilter : ScriptableObject
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}