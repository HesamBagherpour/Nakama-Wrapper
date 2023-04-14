using System;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory
{
    public class ChannelsFactory
    {
        public async UniTask<Tuple<bool, IChannel>> CreateChannel(string userId ,EM_Socket emSocket ,ChannelConfig config)
        {
            IChannel chatChannel = await emSocket.socket.JoinChatAsync(userId, ChannelType.DirectMessage,config.Persistence,config.Hidden);
            return new Tuple<bool, IChannel>(true, chatChannel);
        }
    }
  
}