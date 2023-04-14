using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory
{
    public class ClientFactory 
    {
        private List<EM_Client> _clients = new List<EM_Client>();
        public Action<EM_Client> OnCreateClint;
        public Dictionary<string, Action<EM_Client>> ClientFactoryCallBack = new Dictionary<string, Action<EM_Client>>();

        public string latestTagCreated;
        
        public async UniTask<Tuple<bool, EM_Client>> CreateClint(string tag, ServerClientConfigs config)
        {
            _clients = new List<EM_Client>();
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, EM_Client>(true,client);
            
            client = new EM_Client(tag, config);
            _clients.Add(await client.Init());
            OnCreateClint?.Invoke(client);
            latestTagCreated = tag;
            return new Tuple<bool, EM_Client>(true, client);
        }
        public async UniTask<Tuple<bool, EM_Client>> CreateOrGetClint(string tag, ServerClientConfigs config)
        {
            //TODO check if not exist tag or name - return error if exist
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, EM_Client>(true,client);
            return await CreateClint(tag, config);

        }
        public EM_Client GetClint(string tag)
        {
            return _clients.Find(x => x.tag == tag);
        }
        
        //get clint as Dictionary callBack 
        public async UniTask<EM_Client> GetClintAsync(string tag)
        {

            if (_clients.Exists(x => x.tag == tag))
            {
                return _clients.Find(x => x.tag == tag);
            }
            else
            {
                await UniTask.WaitUntil(() => latestTagCreated == tag );
               return _clients.Find(x => x.tag == tag);
            }
        }
    }
}