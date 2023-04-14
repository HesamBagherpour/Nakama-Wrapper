using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using Infinite8.NakamaWrapper.Scripts.Runtime.Factory;
using Nakama;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Core
{
    public class EM_Client
    {
        public string tag;
        public IClient client;
        public SessionFactory SessionFactory;
        private readonly ServerClientConfigs _clientConfig;

        
        public EM_Client(string tag, ServerClientConfigs clientConfig)
        {
            this.tag = tag;
            this._clientConfig = clientConfig;
            SessionFactory = new SessionFactory();
        }

        public async UniTask<EM_Client> Init()
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
