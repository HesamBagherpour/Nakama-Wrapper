using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchMessageController : MonoBehaviour 
    {
        private bool _isConnected;
        private EM_Socket _socket;
        private string _matchId;
        private Action<long, string, string, IMatchState> _onReceiveOpCodeMessage;
        private List<OpCodeCompModel> _opCodes;
        private  long _lastReceivedGameState;
        private  IMatchState _currentMatchState;
        public Action OnJoinPlayer;
        public MatchOpCodeController matchOpCodeController;
        public void Init(EM_Socket socket ,string matchId)
        {
            _opCodes = new List<OpCodeCompModel>();
            _socket = socket;
            _matchId = matchId;
            matchOpCodeController = new MatchOpCodeController();
            matchOpCodeController.Init(this);
           _socket.socket.ReceivedMatchState += SocketOnReceivedMatchState;
        }
        private void SocketOnReceivedMatchState(IMatchState matchState)
        {
            if(matchState.MatchId != _matchId)
                return;
            _lastReceivedGameState = DateTimeOffset.Now.ToUnixTimeSeconds();
            _currentMatchState = matchState;
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