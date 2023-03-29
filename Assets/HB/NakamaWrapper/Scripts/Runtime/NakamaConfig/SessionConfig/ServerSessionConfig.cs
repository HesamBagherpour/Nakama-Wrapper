using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.SessionConfig
{
    [CreateAssetMenu( menuName = "Infinite8/Nakama/Create New Session Config" , fileName = "ServerSessionConfigs")]
    public class ServerSessionConfigs : ScriptableObject
    {
        public string uniqueIdentifier = "";

    }

}