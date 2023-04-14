using System;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class OpCodeCompModel
    {
        public string Key;

        // [HideInInspector]
        public long OpCode;

        // [HideInInspector]
        public string Uuid;
        public OpCodeUuidGeneratorType UuidGeneratorType = OpCodeUuidGeneratorType.Static;
    }
}
