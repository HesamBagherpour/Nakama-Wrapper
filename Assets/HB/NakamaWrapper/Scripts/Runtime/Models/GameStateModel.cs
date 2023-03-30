using System.Collections.Generic;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
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