using System;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchConnectionController : MonoBehaviour
    {
        public Action<string,string,EM_Socket> OnMatchConnect;
        private EM_Socket _socket;
        public bool isConnected;
        public string matchId;
        private string _matchTag;
        private IMatch _match;
        private MatchConfig _matchConfig;
        public MatchMessageController matchMessageController;
        

        #region Init
            public void Init(EM_Socket socket,MatchConfig matchConfig)
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
                OnMatchConnect?.Invoke(matchId,_matchTag,_socket);
            }

            
            public IMatch GetMatch()
            {
                return _match;
            }
        #endregion

    }
}