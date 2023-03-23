using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using HB.NakamaWrapper.Scripts.Runtime.Modules.Chat;
using Nakama;
using Nakama.TinyJson;
using Newtonsoft.Json;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Utilities
{
    public static class NakamaRpc
    {
        public static async UniTask<string> SendRpc<T>(
            IClient clint,
            ISession session,
            string rpcName,
            float timeoutSec = 0,
            string payload = null,
            RetryConfiguration retryConfiguration = null,
            CancellationToken canceller = default(CancellationToken))
        {
            List<CancellationToken> cancellationTokenList = new List<CancellationToken>();
            cancellationTokenList.Add(canceller);

            CancellationTokenSource timeoutToken = null;
            if (timeoutSec > 0)
            {
                timeoutToken = new CancellationTokenSource();
                timeoutToken.CancelAfter(TimeSpan.FromSeconds(timeoutSec));
                cancellationTokenList.Add(timeoutToken.Token);
            }

            string roomToken = "generalRoomToken";
            MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = roomToken};
            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenList.ToArray());
            var res = await clint.RpcAsync(
                session,
                "JoinOrCreateMatchRpc",
                roomTokenPayload.ToJson(), canceller: linkedTokenSource.Token);
            return res.Payload;
            
            
            // try
            // {
            //     Debug.Log("___________________1111111111111111_" + rpcName);
            //     Debug.Log("___________________1111111111111111_" + payload);
            //     Debug.Log("___________________1111111111111111_" + rpcName);
            //     string roomToken = "generalRoomToken";
            //     MatchLabelFilter roomTokenPayload = new MatchLabelFilter {RoomToken = roomToken};
            //     var linkedTokenSource =
            //         CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenList.ToArray());
            //     var res = await clint.RpcAsync(
            //         session,
            //         "JoinOrCreateMatchRpc",
            //         roomTokenPayload.ToJson());
            //
            //     var resPayload = res.Payload;
            //     var resPayloadDeserialize = JsonConvert.DeserializeObject<T>(res.Payload);
            //     // T resModel = JsonConvert.DeserializeObject<T>(res.Payload);
            //     // linkedTokenSource.Dispose();
            //
            //     Debug.Log("____________________" + res.Payload);
            //     return new GeneralResModel<T>(res.Payload);
            // }
            // catch (OperationCanceledException)
            // {
            //     if (timeoutToken != null && timeoutToken.IsCancellationRequested)
            //     {
            //         Debug.Log("Timeout.");
            //         return new GeneralResModel<T>("Timeout.");
            //     }
            //
            //     if (canceller.IsCancellationRequested)
            //     {
            //         Debug.Log("Cancel clicked.");
            //         return new GeneralResModel<T>("User Canceled.");
            //     }
            // }
            // catch (Exception ex) when (!(ex is OperationCanceledException))
            // {
            //     Debug.Log($"exception message: {ex.Message}");
            //     return new GeneralResModel<T>($"exception message: {ex.Message}");
            // }
            //
            // return new GeneralResModel<T>($"error");
        }

        
    }

    
}