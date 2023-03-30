using System.Runtime.Serialization;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class MetadataModel
    {
        [DataMember(Name = "playerStateJoin")] public ClientState PlayerState;
        [DataMember(Name = "playerAvatar")] public string PlayarAvatar;
        [DataMember(Name = "avatarType")] public string AvatarType;
        public MetadataModel(ClientState playerState ,string playarAvatar , string avatarType)
        {
            PlayerState = playerState;
            PlayarAvatar = playarAvatar;
            AvatarType = avatarType;
        }
        public MetadataModel()
        {
            PlayerState = 0;
            PlayarAvatar = "";
            AvatarType = "";
        }
    }
}