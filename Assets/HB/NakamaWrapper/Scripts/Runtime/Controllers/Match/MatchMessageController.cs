using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchMessageController : MonoBehaviour 
    {
        private bool _isConnected;
        private I8Socket _socket;
        private string _matchId;
        private Action<long, string, string, IMatchState> _onReceiveOpCodeMessage;
        private List<OpCodeCompModel> opCodes;
        protected internal long lastReceivedGameState;
        protected internal IMatchState currentMatchState;
        public Action OnJoinPlayer;

        private long _localPlayerAdd = 0;
        public MatchOpCodeController matchOpCodeController;
        public void Init(I8Socket socket ,MatchOpCodeController matchOpCodeController ,string matchId)
        {
            opCodes = new List<OpCodeCompModel>();
            _socket = socket;
            _matchId = matchId;
            this.matchOpCodeController = matchOpCodeController;
            _socket.socket.ReceivedMatchState += SocketOnReceivedMatchState;
        }

        private void SocketOnReceivedMatchState(IMatchState matchState)
        {
            
            if(matchState.MatchId != _matchId)
                return;
            
            lastReceivedGameState = DateTimeOffset.Now.ToUnixTimeSeconds();
            currentMatchState = matchState;
            
            if (matchOpCodeController.opCodeCallbacks.ContainsKey(matchState.OpCode))
            {
                matchOpCodeController.opCodeCallbacks[matchState.OpCode].Invoke(matchState.OpCode,
                    matchOpCodeController.opCodeKeyByValue.ContainsKey(matchState.OpCode)
                        ? matchOpCodeController.opCodeKeyByValue[matchState.OpCode]
                        : null,
                    matchState);
            }
     
        }


        public void SetMatchId(string matchId)
        {
            _matchId = matchId;
        }
        
        #region SendMatchState
        
        protected internal async UniTask SendMatchState(long opCode, string state,
            IEnumerable<IUserPresence> presences = null)
        {
            // if (!MultiPlayerManager.Instance.isConnected)
            //     return;
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("__________________" + e);
                throw;
            }
        }

        protected internal async UniTask SendMatchState(long opCode, ArraySegment<byte> state,
            IEnumerable<IUserPresence> presences = null)
        {
            // if (!MultiPlayerManager.Instance.isConnected)
            //     return;
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("__________________" + e);
                throw;
            }
        }

        protected internal async UniTask SendMatchState(long opCode, byte[] state,
            IEnumerable<IUserPresence> presences = null)
        {
            // if (!MultiPlayerManager.Instance.isConnected)
            //     return;
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("__________________" + e);
                throw;
            }
        }
        
        
        #endregion
        


        
        
    }
}