using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.NakamaConfig.SessionConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New Session Config" , fileName = "ServerSessionConfigs")]
    public class ServerSessionConfigs : ScriptableObject
    {
        public string uniqueIdentifier = "";

    }

}