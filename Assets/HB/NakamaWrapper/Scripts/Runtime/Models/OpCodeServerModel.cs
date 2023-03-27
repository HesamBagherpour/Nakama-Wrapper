using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
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