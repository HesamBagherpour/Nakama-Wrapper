using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    public class ClientFactory 
    {
        private List<I8Client> _clients = new List<I8Client>();
        public Action<I8Client> OnCreateClint;
        public Dictionary<string, Action<I8Client>> ClientFactoryCallBack = new Dictionary<string, Action<I8Client>>();

        public string latestTagCreated;
        
        public async UniTask<Tuple<bool, I8Client>> CreateClint(string tag, ServerClientConfigs config)
        {
            _clients = new List<I8Client>();
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, I8Client>(true,client);
            
            client = new I8Client(tag, config);
            _clients.Add(await client.Init());
            OnCreateClint?.Invoke(client);
            latestTagCreated = tag;
            return new Tuple<bool, I8Client>(true, client);
        }
        public async UniTask<Tuple<bool, I8Client>> CreateOrGetClint(string tag, ServerClientConfigs config)
        {
            //TODO check if not exist tag or name - return error if exist
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, I8Client>(true,client);
            return await CreateClint(tag, config);

        }
        public I8Client GetClint(string tag)
        {
            return _clients.Find(x => x.tag == tag);
        }
        
        //get clint as Dictionary callBack 
        public async UniTask<I8Client> GetClintAsync(string tag)
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