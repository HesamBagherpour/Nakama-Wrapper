using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class ClientFactory 
    {
        private List<I8Client> _clients;
        public async UniTask<Tuple<bool, I8Client>> CreateClint(string tag, ServerClientConfigs config)
        {
            _clients = new List<I8Client>();
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, I8Client>(false,null);
            
            client = new I8Client(tag, config);
            _clients.Add(await client.Init());
            return new Tuple<bool, I8Client>(true, client);
        }
        public async UniTask<Tuple<bool, I8Client>> CreateOrGetClint(string tag, ServerClientConfigs config)
        {
            //TODO check if not exist tag or name - return error if exist
            var client = _clients.Find(x => x.tag == tag);
            if (client != null)
                return new Tuple<bool, I8Client>(true,client);
            
            client = new I8Client(tag, config);
            _clients.Add(await client.Init());
            return new Tuple<bool, I8Client>(true, client);
        }
        public I8Client GetClint(string tag)
        {
            return _clients.Find(x => x.tag == tag);
        }
    }
}