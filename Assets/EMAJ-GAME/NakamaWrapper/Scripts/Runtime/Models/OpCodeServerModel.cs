using Newtonsoft.Json;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class OpCodeServerModel
    {
        [JsonProperty( "opCode")] public long OpCode;
        [JsonProperty( "key")] public string Key;

        public OpCodeServerModel()
        {
        }

        public OpCodeServerModel(long opCode, string key)
        {
            OpCode = opCode;
            Key = key;
        }
    }
}