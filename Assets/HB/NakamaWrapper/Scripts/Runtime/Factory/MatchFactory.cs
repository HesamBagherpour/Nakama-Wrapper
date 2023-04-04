using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Utilities;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    // I8Match list
    public class MatchFactory
    {
        private List<I8Match> _i8Match = new List<I8Match>();
        public Action<I8Match> OnCreateMatch;
        public string latestTagCreated;
        public async UniTask<Tuple<bool,GeneralResModel<I8Match>>> CreateMatch(string tag,I8Client client, I8Session session ,RpcConfig rpcConfig) 
        {
        
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = rpcConfig.roomToken};
            var matchRes = await NakamaRpc.SendRpc<string>(client.client, session.Session, rpcConfig.rpcName, rpcConfig.timeOutSec,
                 JsonConvert.SerializeObject(roomTokenPayload), new RetryConfiguration(rpcConfig.baseDelayMs, rpcConfig.maxRetries));

            MatchData matchIdData = JsonConvert.DeserializeObject<MatchData>(matchRes);
            I8Match matchData = new I8Match(tag,matchIdData.matchId,client,session);
            _i8Match.Add(matchData);
            OnCreateMatch?.Invoke(matchData);
            latestTagCreated = tag;
            return new Tuple<bool, GeneralResModel<I8Match>>(true, new GeneralResModel<I8Match>(matchData));
            
        }
        //Get - Get or Create 
        
        public async UniTask<Tuple<bool, GeneralResModel<I8Match>>> CreateOrGetClint(string tag,I8Client client, I8Session session ,RpcConfig rpcConfig)
        {
            var match = _i8Match.Find(x => x.tag == tag);
            if (match != null)
                return new Tuple<bool,  GeneralResModel<I8Match>>(true,new GeneralResModel<I8Match>(match));
            
            return await CreateMatch(tag,client, session , rpcConfig);
            
        }
        
        
        public async UniTask<I8Match> GetMatch(string tag)
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