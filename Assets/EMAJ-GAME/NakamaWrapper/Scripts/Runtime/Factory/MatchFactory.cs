using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Utilities;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using Newtonsoft.Json;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory
{
    // I8Match list
    public class MatchFactory
    {
        private List<EM_Match> _i8Match = new List<EM_Match>();
        public Action<EM_Match> OnCreateMatch;
        public string latestTagCreated;
        public async UniTask<Tuple<bool,GeneralResModel<EM_Match>>> CreateMatch(string tag,EM_Client client,
            EM_Session session ,RpcConfig rpcConfig,MatchMessageController matchMessageController ,MatchConnectionController matchConnectionController ) 
        {
        
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = rpcConfig.roomToken};
            var matchRes = await NakamaRpc.SendRpc<string>(client.client, session.Session, rpcConfig.rpcName, rpcConfig.timeOutSec,
                 JsonConvert.SerializeObject(roomTokenPayload), new RetryConfiguration(rpcConfig.baseDelayMs, rpcConfig.maxRetries));

            MatchData matchIdData = JsonConvert.DeserializeObject<MatchData>(matchRes);
            EM_Match matchData = new EM_Match(tag,matchIdData.matchId,client,session,matchMessageController,matchConnectionController);
            _i8Match.Add(matchData);
            OnCreateMatch?.Invoke(matchData);
            latestTagCreated = tag;
            return new Tuple<bool, GeneralResModel<EM_Match>>(true, new GeneralResModel<EM_Match>(matchData));
             
        }
        //Get - Get or Create 
        
        public async UniTask<Tuple<bool, GeneralResModel<EM_Match>>> CreateOrGetClint(string tag,EM_Client client,
            EM_Session session ,RpcConfig rpcConfig,MatchMessageController matchMessageController ,MatchConnectionController matchConnectionController)
        {
            var match = _i8Match.Find(x => x.tag == tag);
            if (match != null)
                return new Tuple<bool,  GeneralResModel<EM_Match>>(true,new GeneralResModel<EM_Match>(match));
            
            return await CreateMatch(tag,client, session , rpcConfig , matchMessageController , matchConnectionController);
            
        }
        
        
        public async UniTask<EM_Match> GetMatch(string tag)
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