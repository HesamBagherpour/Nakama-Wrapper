using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public class GameStateModel
    {
        [JsonProperty("players")] public List<PlayerModel> Players;
        [JsonProperty( "opCodes")] public List<OpCodeServerModel> OpCodes;

        public GameStateModel()
        {
        }

        public GameStateModel(List<PlayerModel> players, List<OpCodeServerModel> opCodes)
        {
            Players = players;
            OpCodes = opCodes;
        }
    }
}