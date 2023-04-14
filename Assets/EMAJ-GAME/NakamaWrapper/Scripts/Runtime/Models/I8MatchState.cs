using System;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8MatchState
    {
        public  event Action<IMatchState> I8OnReceivedChannelMessage;

        public void Raise(IMatchState channelMessage);


    }
}