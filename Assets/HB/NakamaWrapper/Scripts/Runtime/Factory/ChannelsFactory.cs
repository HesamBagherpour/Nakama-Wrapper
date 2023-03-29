using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    public class ChannelsFactory
    {
        public async UniTask<Tuple<bool, IChannel>> CreateChannel(string userId ,I8Socket i8Socket ,ChannelConfig config)
        {
            IChannel chatChannel = await i8Socket.socket.JoinChatAsync(userId, ChannelType.DirectMessage,config.Persistence,config.Hidden);
            return new Tuple<bool, IChannel>(true, chatChannel);
        }
    }
  
}