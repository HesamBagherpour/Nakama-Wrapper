using Newtonsoft.Json;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public class MatchLabelFilter
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}