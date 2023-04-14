using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;


namespace Infinite8.NakamaWrapper.Scripts.Runtime.Factory
{
    public class MatchMakingFactory
    {
        
        private List<EM_MatchmakerTicket> _i8Match = new List<EM_MatchmakerTicket>();
        public Action<IMatchmakerTicket> OnCreateMatch;
        public string latestTagCreated;
        public string currentMatchmakingTicket;
        public async UniTask<Tuple<bool,GeneralResModel<EM_MatchmakerTicket>>> CreateMatchMaking(string tag,EM_Socket socket,MatchMakingConfig matchMakingConfig) 
        {
            IMatchmakerTicket matchmakerTicket = await socket.socket.AddMatchmakerAsync(matchMakingConfig.query,
                    matchMakingConfig.minPlayers, matchMakingConfig.maxPlayers, matchMakingConfig.matchmakingProperties);
            currentMatchmakingTicket = matchmakerTicket.Ticket;
            EM_MatchmakerTicket emMatchmakerTicket = new EM_MatchmakerTicket();
            emMatchmakerTicket.MatchmakerTicket = matchmakerTicket;
            emMatchmakerTicket.tag = tag;
            _i8Match.Add(emMatchmakerTicket);
            latestTagCreated = tag;
            return new Tuple<bool, GeneralResModel<EM_MatchmakerTicket>>(true, new GeneralResModel<EM_MatchmakerTicket>(emMatchmakerTicket));
        }
        public async UniTask<Tuple<bool, GeneralResModel<EM_MatchmakerTicket>>> CreateOrGetClint(string tag, EM_Socket socket ,MatchMakingConfig matchMakingConfig)
        {
            var match = _i8Match.Find(x => x.tag == tag);
            if (match != null)
                return new Tuple<bool,  GeneralResModel<EM_MatchmakerTicket>>(true,new GeneralResModel<EM_MatchmakerTicket>(match));
            return await CreateMatchMaking(tag,socket , matchMakingConfig);
        }
        public async UniTask<EM_MatchmakerTicket> GetMatchmakerTicket(string tag)
        {

            if (_i8Match.Exists(x => x.tag == tag))
            {
                return _i8Match.Find(x => x.tag == tag);
            }
            else
            {
                await UniTask.WaitUntil(() => latestTagCreated == tag );
                return _i8Match.Find(x => x.tag == tag);
            }
        }
    }


}