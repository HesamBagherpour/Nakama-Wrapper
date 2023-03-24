using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class ChannelsFactory
    {
        public async UniTask<Tuple<bool, IChannel>> CreateChannel(string userId ,HSocket hSocket ,ChannelConfig config)
        {
            IChannel chatChannel = await hSocket.socket.JoinChatAsync(userId, ChannelType.DirectMessage,config.Persistence,config.Hidden);
            return new Tuple<bool, IChannel>(true, chatChannel);
        }
    }
  
}