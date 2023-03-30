using System.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public class I8Socket
    {
        public ISocket socket;
        public I8Client client;
        public string tag;
        public SocketConfig SocketConfig;
        public I8Socket(string tag,I8Client client ,SocketConfig socketConfig)
        {
            this.tag = tag;
            this.client = client;
            SocketConfig = socketConfig;
        }
        public async Task<I8Socket> Init()
        {
            socket = client.client.NewSocket(true);
            return this;
        }

    }
}