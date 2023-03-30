using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
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
        private List<OpCodeCompModel> _opCodes;
        private  long _lastReceivedGameState;
        private  IMatchState _currentMatchState;
        public Action OnJoinPlayer;
        [SerializeField] private long localPlayerAdd = 0;
        public MatchOpCodeController MatchOpCodeController;
        public void Init(I8Socket socket ,MatchOpCodeController matchOpCodeController ,string matchId)
        {
            _opCodes = new List<OpCodeCompModel>();
            _socket = socket;
            _matchId = matchId;
            MatchOpCodeController = matchOpCodeController;
            _socket.socket.ReceivedMatchState += SocketOnReceivedMatchState;
        }
        private void SocketOnReceivedMatchState(IMatchState matchState)
        {
            if(matchState.MatchId != _matchId)
                return;
            _lastReceivedGameState = DateTimeOffset.Now.ToUnixTimeSeconds();
            _currentMatchState = matchState;
            
            if (MatchOpCodeController.opCodeCallbacks.ContainsKey(matchState.OpCode))
            {
                MatchOpCodeController.opCodeCallbacks[matchState.OpCode].Invoke(matchState.OpCode,
                    MatchOpCodeController.opCodeKeyByValue.ContainsKey(matchState.OpCode)
                        ? MatchOpCodeController.opCodeKeyByValue[matchState.OpCode]
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
            
            if(!_socket.socket.IsConnected)
                return;

            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("SendMatchState  error : " + e);
                throw;
            }
        }

        protected internal async UniTask SendMatchState(long opCode, ArraySegment<byte> state,
            IEnumerable<IUserPresence> presences = null)
        {

            if(!_socket.socket.IsConnected)
                return;
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("SendMatchState  error :  " + e);
                throw;
            }
        }

        protected internal async UniTask SendMatchState(long opCode, byte[] state,
            IEnumerable<IUserPresence> presences = null)
        {
            if(!_socket.socket.IsConnected)
                return;
            try
            {
                await _socket.socket.SendMatchStateAsync(_matchId, opCode, state, presences);
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log("SendMatchState  error : " + e);
                throw;
            }
        }
        
        
        #endregion
        
    }
}