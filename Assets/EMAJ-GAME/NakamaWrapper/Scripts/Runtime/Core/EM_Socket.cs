using System.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Factory;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Core
{
    public class EM_Socket
    {
        public ISocket socket;
        public EM_Client client;
        public string tag;
        public SocketConfig SocketConfig;
        public MatchFactory MatchFactory = new MatchFactory();

        public EM_Socket(string tag,EM_Client client ,SocketConfig socketConfig)
        {
            this.tag = tag;
            this.client = client;
            SocketConfig = socketConfig;
        }
        public EM_Socket(string tag,EM_Client client ,SocketConfig socketConfig, MatchMessageController matchMessageController )
        {
            this.tag = tag;
            this.client = client;
            SocketConfig = socketConfig;

        }
        public EM_Socket(string tag,EM_Client client ,SocketConfig socketConfig, MatchMessageController matchMessageController ,MatchConnectionController matchConnectionController)
        {
            this.tag = tag;
            this.client = client;
            SocketConfig = socketConfig;
        }
        public async Task<EM_Socket> Init()
        {
            socket = client.client.NewSocket(true);
            return this;
        }

    }
}