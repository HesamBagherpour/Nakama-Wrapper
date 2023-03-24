using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class ClientFactory 
    {
        private List<HClient> _clients;
        public async UniTask<Tuple<bool, HClient>> CreateClint(string tag, ServerClientConfigs config)
        {
            _clients = new List<HClient>();
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, HClient>(false,null);
            
            client = new HClient(tag, config);
            _clients.Add(await client.Init());
            return new Tuple<bool, HClient>(true, client);
        }
        public async UniTask<Tuple<bool, HClient>> CreateOrGetClint(string tag, ServerClientConfigs config)
        {
            //TODO check if not exist tag or name - return error if exist
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, HClient>(true,client);
            
            client = new HClient(tag, config);
            _clients.Add(await client.Init());
            return new Tuple<bool, HClient>(true, client);
        }
        public HClient GetClint(string tag)
        {
            return _clients.Find(x => x.tag == tag);
        }
    }
}