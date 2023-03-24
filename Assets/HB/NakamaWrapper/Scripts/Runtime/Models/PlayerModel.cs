using System.Runtime.Serialization;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class PlayerModel
    {
        [DataMember(Name = "presence")] public MinimalUserPresence Presence;
        [DataMember(Name = "displayName")] public string DisplayName;
        [DataMember(Name = "avatarId")] public string avatarId;
        [DataMember(Name = "playerStateJoin")] public string PlayerState;
        [DataMember(Name = "metaData")] public MetadataModel MetaData;
    }
}