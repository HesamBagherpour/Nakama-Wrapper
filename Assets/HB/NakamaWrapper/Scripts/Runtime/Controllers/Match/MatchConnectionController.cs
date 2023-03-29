using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchConnectionController : MonoBehaviour
    {
        private I8Socket _socket;
        public bool isConnected;
        public string _matchId;
        public Action<string> OnMatchConnect;

        private IMatch match;

        #region Init
            public void Init(I8Socket socket)
            {
                _socket = socket;
            }
            public async UniTask ConnectMatch(string matchId)
            {
                _matchId = matchId;
                match = await _socket.socket.JoinMatchAsync(matchId);
                
                OnMatchConnect?.Invoke(_matchId);
            }

            public IMatch GetMatch()
            {
                return match;
            }
        #endregion

    }
}