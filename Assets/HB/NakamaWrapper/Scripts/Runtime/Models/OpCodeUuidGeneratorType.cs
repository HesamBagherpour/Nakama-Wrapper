using System;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public enum OpCodeUuidGeneratorType
    {
        Static,
        ServerConfig,
        MultiPlayerNetwork
    }
}