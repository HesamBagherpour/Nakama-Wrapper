using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public class PlayerModel
    {
        [JsonProperty( "presence")] public MinimalUserPresence Presence;
        [JsonProperty( "displayName")] public string DisplayName;
        [JsonProperty( "avatarId")] public string avatarId;
        [JsonProperty( "playerStateJoin")] public string PlayerState;
        [JsonProperty( "metaData")] public MetadataModel MetaData;
    }
}