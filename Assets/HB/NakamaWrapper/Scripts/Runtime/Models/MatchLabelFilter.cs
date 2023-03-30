using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class MatchLabelFilter
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}