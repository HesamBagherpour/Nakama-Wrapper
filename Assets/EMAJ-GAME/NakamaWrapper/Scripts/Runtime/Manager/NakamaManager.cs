using Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Manager
{
    public class NakamaManager : MonoBehaviour
    {
        public ClientFactory ClientFactory;
        public static NakamaManager Instance;
        private void Awake()
        {
            Instance = this;
            ClientFactory = new ClientFactory();
        }


    }
}