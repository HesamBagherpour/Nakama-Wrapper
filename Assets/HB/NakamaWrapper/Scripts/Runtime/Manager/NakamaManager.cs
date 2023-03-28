using HB.NakamaWrapper.Scripts.Runtime.Controller;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Manager
{
    public class NakamaManager : MonoBehaviour
    {
        public ClientFactory ClientFactory;
        public static NakamaManager Instance;
        public GameObject localPlayer;
        public GameObject remotePlayer;
        public string userId;
        private void Awake()
        {
            Instance = this;
            ClientFactory = new ClientFactory();
        }


    }
}