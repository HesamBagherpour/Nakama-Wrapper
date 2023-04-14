using Newtonsoft.Json;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class MatchLabelFilter
    {
        [JsonProperty("roomToken")] public string RoomToken;
    }
}