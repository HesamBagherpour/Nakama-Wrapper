using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class MatchConnectionController : MonoBehaviour
    {
        private HSocket _socket;
        private bool _isConnected;
        public string _matchId;
        public Action<string> OnMatchConnect;


        #region Init
            public void Init(HSocket socket)
            {
                _socket = socket;
            }
            public async UniTask ConnectMatch(string matchId)
            {
                _matchId = matchId;
                await _socket.socket.JoinMatchAsync(matchId);
                OnMatchConnect?.Invoke(_matchId);
            }
        #endregion

    }
}