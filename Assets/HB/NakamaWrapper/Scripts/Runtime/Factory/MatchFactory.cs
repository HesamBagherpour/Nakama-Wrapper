using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using HB.NakamaWrapper.Scripts.Runtime.Utilities;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    // I8Match list
    public class MatchFactory
    {
        private List<I8Match> _i8Match = new List<I8Match>();
        public async UniTask<Tuple<bool,GeneralResModel<I8Match>>> CreateMatch(string tag,I8Client client, I8Session session ,RpcConfig rpcConfig) 
        {
        
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = rpcConfig.roomToken};
            var matchRes = await NakamaRpc.SendRpc<string>(client.client, session.Session, rpcConfig.rpcName, rpcConfig.timeOutSec,
                 JsonConvert.SerializeObject(roomTokenPayload), new RetryConfiguration(rpcConfig.baseDelayMs, rpcConfig.maxRetries));

            
            MatchData matchIdData = JsonConvert.DeserializeObject<MatchData>(matchRes);
            
            // Create new I8Match and fill variable  -> tag ...
            
            I8Match matchData = new I8Match(tag,matchIdData.matchId,client,session);
            
            _i8Match.Add(matchData);
            return new Tuple<bool, GeneralResModel<I8Match>>(true, new GeneralResModel<I8Match>(matchData));
            
        }
        //Get - Get or Create 
        
        public async UniTask<Tuple<bool, GeneralResModel<I8Match>>> CreateOrGetClint(string tag,I8Client client, I8Session session ,RpcConfig rpcConfig)
        {
            //TODO check if not exist tag or name - return error if exist
            I8Match matchData = _i8Match.Find(x => x.tag == tag);
            if (matchData != null)
                return new Tuple<bool, GeneralResModel<I8Match>>(true, new GeneralResModel<I8Match>(matchData));
            
            
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = rpcConfig.roomToken};
            var matchRes = await NakamaRpc.SendRpc<string>(client.client, session.Session, rpcConfig.rpcName, rpcConfig.timeOutSec,
                JsonConvert.SerializeObject(roomTokenPayload), new RetryConfiguration(rpcConfig.baseDelayMs, rpcConfig.maxRetries));
            
            MatchData matchIdData = JsonConvert.DeserializeObject<MatchData>(matchRes);

            I8Match createdMatchData = new I8Match(tag,matchIdData.matchId,client,session);
            _i8Match.Add(createdMatchData);
            return new Tuple<bool, GeneralResModel<I8Match>>(true, new GeneralResModel<I8Match>(matchData));
        }
        
        
        public I8Match GetMatch(string tag)
        {
            return _i8Match.Find(x => x.tag == tag);
        }

    }
}