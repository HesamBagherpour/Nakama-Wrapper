using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchConnectionController : MonoBehaviour
    {
        public Action<string,string> OnMatchConnect;
        private I8Socket _socket;
        public bool isConnected;
        public string matchId;
        private string _matchTag;
        private IMatch _match;
        private MatchConfig _matchConfig;
        public MatchMessageController matchMessageController;
        

        #region Init
            public void Init(I8Socket socket,MatchConfig matchConfig)
            {
                _socket = socket;
                _matchConfig = matchConfig;
            }
            public async UniTask ConnectMatch(string _matchId,MatchMessageController matchMessageController)
            {
                this.matchId = _matchId;
                _match = await _socket.socket.JoinMatchAsync(matchId);
                isConnected = true;
                string matchTag = _matchConfig.GETMatchName();
                this.matchMessageController = matchMessageController;
               //NakamaManager.Instance.OnMatchConnected?.Invoke(matchTag,this);
                OnMatchConnect?.Invoke(this.matchId,_matchTag);
            }

            public IMatch GetMatch()
            {
                return _match;
            }
        #endregion

    }
}