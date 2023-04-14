using Newtonsoft.Json;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class MinimalUserPresence
    {
        [JsonProperty( "username")] public string Username;
        [JsonProperty( "userId")] public string UserId;
        [JsonProperty( "sessionId")] public string SessionId;

        public MinimalUserPresence()
        {
        }

        public MinimalUserPresence(string username, string userId, string sessionId)
        {
            Username = username;
            UserId = userId;
            SessionId = sessionId;
        }
    }
}