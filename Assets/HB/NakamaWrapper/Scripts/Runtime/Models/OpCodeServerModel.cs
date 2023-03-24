using System.Runtime.Serialization;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public class OpCodeServerModel
    {
        [DataMember(Name = "opCode")] public long OpCode;
        [DataMember(Name = "key")] public string Key;
    }
}