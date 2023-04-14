using System;
using System.Collections.Generic;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Socket;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace Emaj_Game.UnitySample.Scripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        // Start is called before the first frame update
        private EM_Socket _emSocket;
        public string matchId;
        public MatchConnectionController matchConnectionController;
        public SocketConnectionController SocketConnectionController;
        public GameObject player;
        public Dictionary<string, GameObject> playerAvatar = new Dictionary<string, GameObject>();
        public Action<string> playerJoined;
        public Action<string> playerDisconnected;
        public Action<bool> PlayerJoinMatch;


        private void Awake()
        {
            SocketConnectionController.OnConnectSocket += delegate(EM_Socket socket)
            {
                socket.socket.ReceivedMatchState += SocketOnReceivedMatchState;
                socket.socket.ReceivedMatchPresence += SocketOnReceivedMatchPresence;
                socket.socket.ReceivedMatchmakerMatched += SocketOnReceivedMatchmakerMatched;
                _emSocket = socket;
            };
            matchConnectionController.OnMatchConnect += OnMatchConnect;
        }

        private void Start()
        {

        
        
        }
        private void SocketOnReceivedMatchmakerMatched(IMatchmakerMatched obj)
        {
        
        }
        private void SocketOnReceivedMatchState(IMatchState matchState)
        {
            switch (matchState.OpCode)
            {
                case 0:
                    var avatar = Instantiate(player);
                    var localCharacterController = avatar.GetComponent<NakamaCharacterController>();
                    avatar.GetComponent<Renderer>().material.color  = Color.green;
                    localCharacterController.isLocalPlayer = false;
                    // localCharacterController.playerUserId = matchState.UserPresence.UserId;
                    // playerAvatar.Add(matchState.UserPresence.UserId,avatar);

                
                    break;
                case 1:
                    var opAvatar = Instantiate(player);
                    var oPcharacterController = opAvatar.GetComponent<NakamaCharacterController>();
                    oPcharacterController.GetComponent<Renderer>().material.color  = Color.red;
                    oPcharacterController.isLocalPlayer = false;
                    oPcharacterController.playerUserId = matchState.UserPresence.UserId;
                    playerAvatar.Add(matchState.UserPresence.UserId,opAvatar);
                
                    break;
                
            }

        }
        private void OnMatchConnect(string matchId, string matchTag, EM_Socket socket)
        {

            PlayerJoinMatch?.Invoke(true);
        }
        private void SocketOnReceivedMatchPresence(IMatchPresenceEvent presenceEvent)
        {

            foreach (var joined in presenceEvent.Joins)
            {

                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasssssssssss");
                playerJoined?.Invoke(joined.UserId);
            
            }
            foreach (var left in presenceEvent.Leaves)
            {

                playerDisconnected?.Invoke(left.UserId);
            
            }
       
        }
    

    }
}
