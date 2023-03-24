using System.Runtime.Serialization;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class MinimalUserPresence
    {
        [DataMember(Name = "username")] public string Username;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "sessionId")] public string SessionId;
    }
}