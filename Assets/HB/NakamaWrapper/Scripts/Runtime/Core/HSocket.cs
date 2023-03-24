using System.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public class HSocket
    {
        public ISocket socket;
        public HClient client;
        public string tag;
        public SocketConfig SocketConfig;
        public HSocket(string tag,HClient client ,SocketConfig socketConfig)
        {
            this.tag = tag;
            this.client = client;
            SocketConfig = socketConfig;
        }
        public async Task<HSocket> Init()
        {
            socket = client.client.NewSocket(true);
            return this;
        }

    }
}