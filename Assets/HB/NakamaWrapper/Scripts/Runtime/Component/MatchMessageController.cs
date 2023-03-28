using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Nakama.TinyJson;
using Newtonsoft.Json;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class MatchMessageController : MonoBehaviour 
    {
        private bool _isConnected;
        private HSocket _socket;
        private string _userName;
        private string _matchId;
        private string _userId;
        private Action<long, string, string, IMatchState> _onReceiveOpCodeMessage;
        private List<OpCodeCompModel> opCodes;
        protected internal long lastReceivedGameState;
        protected internal IMatchState currentMatchState;
        public Action<bool,bool,string> OnJoinPlayer;
        public Action<MoveStateModelNew,string> OnPlayerPosition;

        private long _localPlayerAdd = 0;
        
        public void Init(HSocket socket ,string userName ,string matchId , string userId)
        {
            opCodes = new List<OpCodeCompModel>();
            _socket = socket;
            _userName = userName;
            _matchId = matchId;
            _userId = userId;
            _socket.socket.ReceivedMatchState += SocketOnReceivedMatchState;
            
        }

        private void SocketOnReceivedMatchState(IMatchState matchState)
        {
            lastReceivedGameState = DateTimeOffset.Now.ToUnixTimeSeconds();
            currentMatchState = matchState;
            var state = matchState.State.Length > 0 ? System.Text.Encoding.UTF8.GetString(matchState.State) : null;
            switch (matchState.OpCode)
            {
                    case 0:
                    Debug.Log("other player Joined");
                    Debug.Log("state : " + state);
                    GameStateModel playrs = JsonConvert.DeserializeObject<GameStateModel>(state);
                    foreach (var player in playrs.Players)
                    {
                        if (player.Presence.UserId == _userId)
                        {
                            Debug.Log("this is Me ");
                            OnJoinPlayer?.Invoke(true,false,player.Presence.UserId);
                        }
                        else
                        {
                            Debug.Log(" this is Other Player ");
                            OnJoinPlayer?.Invoke(false,false,player.Presence.UserId);
                        }
                    }
                    break;
                    case 1: 
                        Debug.Log("other player Joined");
                        PlayerModel otherPlayers = JsonConvert.DeserializeObject<PlayerModel>(state);
                        OnJoinPlayer?.Invoke(false,true,otherPlayers.Presence.UserId);
                        break;
                    case 200:
                        
                        var packet = JsonConvert.DeserializeObject<MultiPlayerMessage<MoveStateModelNew>>(Encoding.UTF8.GetString(matchState.State));
                        if (packet != null) Debug.Log("stateDictionary   :  " + packet.message.pos + "aaaaaaaa"  + packet.uuid);
                        if (packet != null) OnPlayerPosition?.Invoke(packet.message , matchState.UserPresence.UserId);

                        break;

            }
            
            

     

            // if (state != null)
            // {
            //     switch (matchState.OpCode)
            //     {
            //         case 0:
            //             var gameState = state.FromJson<GameStateModel>();
            //             gameState.Players = gameState.Players.OrderByDescending(p =>
            //                 p.Presence.UserId == _userId).ToList();
            //             Debug.unityLogger.Log(gameState.Players.Count);
            //             
            //             bool isCreateLocalPlayer = false;
            //             foreach (var player in gameState.Players)
            //             {
            //                 if (player is null) continue;
            //                 // if (player.presence.userId == MultiPlayerManager.Instance.currentServer.GetUserId())
            //                 // continue;
            //                 //player.Presence.AvatarId
            //             
            //                 
            //                 
            //                 if (player.MetaData == null)
            //                     player.MetaData = new MetadataModel();
            //
            //                 OnJoinPlayer?.Invoke();
            //                 if (player.Presence.UserId == _userId)
            //                     isCreateLocalPlayer = true;
            //             
            //                 // if (!PlayerSpawner.Instance.players.ContainsKey(player.Presence.UserId))
            //                 // {
            //                 //     _nakamaMultiPlayerController.OnJoinPlayer?.Invoke(player);
            //                 //     if (player.Presence.UserId == MultiPlayerManager.Instance.currentServer.GetUserId())
            //                 //         isCreateLocalPlayer = true;
            //                 // }
            //             }
            //             
            //             // if (isCreateLocalPlayer)
            //             //     PlayerSpawner.Instance.reposLocalPlayer();
            //             //
            //             // Debug.unityLogger.Log("Players came");
            //             break;
            //         case 1:
            //             var playerJoint = state.FromJson<PlayerModel>();
            //             // Debug.unityLogger.Log("PlayerJoint came");
            //             // if (!PlayerSpawner.Instance.players.ContainsKey(playerJoint.Presence.UserId))
            //             // {
            //             //     if (playerJoint.MetaData == null)
            //             //         playerJoint.MetaData = new MetadataModel();
            //             //
            //             //     _nakamaMultiPlayerController.OnJoinPlayer?.Invoke(playerJoint);
            //             //     if (playerJoint.Presence.UserId == MultiPlayerManager.Instance.currentServer.GetUserId())
            //             //         PlayerSpawner.Instance.reposLocalPlayer();
            //             // }
            //
            //             break;
            //         default:
            //             // currentMatchState = matchState;
            //             // if (_nakamaMultiPlayerController.opCodeCallbacks.ContainsKey(matchState.OpCode))
            //             // {
            //             //     _nakamaMultiPlayerController.opCodeCallbacks[matchState.OpCode].Invoke(matchState.OpCode,
            //             //         _nakamaMultiPlayerController.opCodeKeyByValue.ContainsKey(matchState.OpCode)
            //             //             ? _nakamaMultiPlayerController.opCodeKeyByValue[matchState.OpCode]
            //             //             : null,
            //             //         matchState);
            //             // }
            //
            //             break;
            //     }
            // }
            //
            //
            

            //     NakamaManager.Instance.LoadAvatar();
            //
            //     if (obj.OpCode == _localPlayerAdd)
            //     {
            //         Debug.Log("_____________________________________________stateDictionary   :  " + obj);
            //
            //         // var localPlayer = Instantiate(NakamaManager.Instance.avatar,
            //         //     NakamaManager.Instance.localPlayer.transform);
            //
            //         //localPlayer.transform.parent = NakamaManager.Instance.localPlayer.transform;
            //     }
            //
            //     if (obj.OpCode == 1)
            //     {
            //         Debug.Log("______________________________________ddddd_______stateDictionary   :  " + obj);
            //     }
            //     // if (obj.OpCode == 300)
            //     // {
            //     //     var stateDictionary = GetStateAsDictionary(obj.State);
            //     //     Debug.Log("stateDictionary   :  " + stateDictionary["test"]);
            //     // }
            //     //
            //     // if (obj.OpCode == 200)
            //     // {
            //     //     var packet = JsonConvert.DeserializeObject<MultiPlayerMessage<MoveStateModelNew>>(Encoding.UTF8.GetString(obj.State));
            //     //     if (packet != null) Debug.Log("stateDictionary   :  " + packet.message.pos);
            //     // }
            // }
        }


        public void SetMatchId(string matchId)
        {
            _matchId = matchId;
        }
        
        #region SendMatchState

        public async UniTask SendMatchState(long opCode, string state, IEnumerable<IUserPresence> presences = null)
        {
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.LogException(e);
            }
        }
        
        
        
        
        #endregion
        #region ReceiveOpCode
            public void ONReceiveOpCodeMessage(long opCode, string key, IMatchState state)
            {
                var a = JsonConvert.DeserializeObject<MultiPlayerMessage<object>>(Encoding.UTF8.GetString(state.State),
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                if (opCodes.Exists(model => model.Uuid == a.uuid))
                    _onReceiveOpCodeMessage?.Invoke(opCode, key, a.uuid, state);
            }
        #endregion
        
        
        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }


        
        
    }
}