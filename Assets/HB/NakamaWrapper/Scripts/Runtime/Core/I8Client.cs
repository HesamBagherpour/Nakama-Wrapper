using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Factory;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public class I8Client
    {
        public string tag;
        public IClient client;
        public SessionFactory SessionFactory;
        private readonly ServerClientConfigs _clientConfig;

        
        public I8Client(string tag, ServerClientConfigs clientConfig)
        {
            this.tag = tag;
            this._clientConfig = clientConfig;
            SessionFactory = new SessionFactory();
        }

        public async UniTask<I8Client> Init()
        {
            Debug.Log("clintItem" );
            client = new Client(_clientConfig.scheme,
                _clientConfig.host,
                _clientConfig.port,
                _clientConfig.serverKey,
                UnityWebRequestAdapter.Instance,
                _clientConfig.autoRefreshSession);

            Debug.Log(await CallRpcAsync());
            SessionFactory = new SessionFactory();
            
            return this;
        }
        


        public async UniTask<string> CallRpcAsync()
        {
            
            var response = await client.RpcAsync("defaulthttpkey","test", null);
            return response.Payload;
        }
  
        
        

    }
}
