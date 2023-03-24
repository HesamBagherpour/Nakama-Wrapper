using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Utilities;
using Nakama;
using Newtonsoft.Json;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class MatchFactory
    {
        public async UniTask<Tuple<bool, GeneralResModel<MatchData>>> CreateMatch(string tag,HClient client, HSession session ,RpcConfig rpcConfig) 
        {
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = rpcConfig.roomToken};
            var matchRes = await NakamaRpc.SendRpc<string>(client.client, session.Session, rpcConfig.rpcName, rpcConfig.timeOutSec,
                 JsonConvert.SerializeObject(roomTokenPayload), new RetryConfiguration(rpcConfig.baseDelayMs, rpcConfig.maxRetries));

            MatchData app = JsonConvert.DeserializeObject<MatchData>(matchRes);

            return new Tuple<bool,GeneralResModel<MatchData>>(true,new GeneralResModel<MatchData>(app));
        }
    }
}