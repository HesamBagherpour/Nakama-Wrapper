using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Socket
{
    public class SocketPingPongConnection : MonoBehaviour , I8MatchState ,I8ChannelState
    {
        public event Action<IMatchState> I8OnReceivedChannelMessage;
        public event Action<IChannelPresenceEvent> I8OnChannelPresenceEvent;
        private ISocket _socket;
        private PingPongConfig _pingPongConfig;
        private bool _startPingPongConnection;
        private bool _waiting;
        
        public async UniTask<bool> Init(I8Socket socket)
        {

            _pingPongConfig = new PingPongConfig();
            _pingPongConfig.PingPongIntervalSec = 3;
            I8OnReceivedChannelMessage +=OnI8OnReceivedChannelMessage;
            I8OnChannelPresenceEvent +=OnI8OnChannelPresenceEvent;
            while (!_waiting)
            {
                _waiting = true;
                await UniTask.Yield();
            }
            return true;
            
        }

        private void OnI8OnChannelPresenceEvent(IChannelPresenceEvent obj)
        {
            Debug.Log("stateDictionary   :  " +obj);
        }

        private void OnI8OnReceivedChannelMessage(IMatchState obj)
        {
           
            if (obj.OpCode == 3)
            {
               
                Debug.Log("stateDictionary   :  ");
            }
        }



        
     
        #region pingPong

            public void StartPingPong()
            {
                Debug.Log("Start Ping Pong");
                SendPingPong();
            }


            private void SendPingPong(string matchId , SocketConfig socketConfig)
            {
                string MatchId = matchId;
                var a = UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_pingPongConfig.PingPongIntervalSec))
                    .ForEachAsync(_ => { SendGameState(MatchId,3, "", null).Forget();});
            }
            private void SendPingPong()
            {
              
                var a = UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(_pingPongConfig.PingPongIntervalSec))
                    .ForEachAsync(_ => { Debug.Log("StartPingPong  : " );});
            }


        #endregion

        #region SendGameState
            private async UniTask SendGameState(string matchId, long opCode, string state,
                IEnumerable<IUserPresence> presences = null)
            {

                try
                {
                    await _socket.SendMatchStateAsync(matchId, opCode, state, presences);
                }
                catch (Exception e)
                {
                    Debug.unityLogger.Log("Exception Error Socket is close :  " + e);
                    throw;

                }

            }
            
            private async UniTask SendGameState(long opCode, string state,
                IEnumerable<IUserPresence> presences = null)
            {

                try
                {
                    //await _socket.SendMatchStateAsync(matchId, opCode, state, presences);
                }
                catch (Exception e)
                {
                    Debug.unityLogger.Log("Exception Error Socket is close :  " + e);
                    throw;

                }

        }

        

        #endregion
        
        #region Raise
            public void Raise(IMatchState channelMessage)
            {
                I8OnReceivedChannelMessage?.Invoke(channelMessage);
            }

            
            public void Raise(IChannelPresenceEvent channelMessage)
            {
                I8OnChannelPresenceEvent?.Invoke(channelMessage);
            }
        

        #endregion

        
        

    }
}