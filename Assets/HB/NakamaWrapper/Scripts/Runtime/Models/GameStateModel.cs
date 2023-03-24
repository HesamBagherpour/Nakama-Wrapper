using System.Collections.Generic;
using System.Runtime.Serialization;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public abstract class GameStateModel
    {
        [DataMember(Name = "players")] public List<PlayerModel> Players;
        [DataMember(Name = "opCodes")] public List<OpCodeServerModel> OpCodes;
    }
}