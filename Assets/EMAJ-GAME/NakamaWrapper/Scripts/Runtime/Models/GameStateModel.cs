using System.Collections.Generic;
using Newtonsoft.Json;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
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