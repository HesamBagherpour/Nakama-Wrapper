using HB.NakamaWrapper.Scripts.Runtime.Factory;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Manager
{
    public class NakamaManager : MonoBehaviour
    {
        public ClientFactory ClientFactory;
        public static NakamaManager Instance;
        public GameObject localPlayer;
        public GameObject remotePlayer;
        // public Action<string> OnSessionConnected;
        // public Action<string,MatchConnectionController> OnMatchConnected;
        private void Awake()
        {
            Instance = this;
            ClientFactory = new ClientFactory();
        }


    }
}