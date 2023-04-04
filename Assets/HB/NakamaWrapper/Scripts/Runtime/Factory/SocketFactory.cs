using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    public class SocketFactory
    {
        private List<I8Socket> _socketList = new List<I8Socket>();
        public Action<I8Socket> OnCreateSocket;
        private string latestTagCreated;
        public SocketFactory()
        {
            _socketList = new List<I8Socket>();
        }
        public async UniTask<Tuple<bool, I8Socket>> CreateSocket(string tag,I8Client client,SocketConfig config)
        {
            
            //TODO check if not exist tag or name - return error if exist
            I8Socket socket;
            socket = _socketList.Find(x => x.tag == tag);
            if (socket != null)
                return new Tuple<bool, I8Socket>(true,socket); // this socket already exist with same name 
            
            socket = new I8Socket(tag,client,config);
            socket = await socket.Init();
            _socketList.Add(socket);
            OnCreateSocket?.Invoke(socket);
            latestTagCreated = tag;
            return new Tuple<bool, I8Socket>(true, socket);
        }
        
        
        public async UniTask<Tuple<bool, I8Socket>> CreateOrGetSocket(string tag,I8Client client ,SocketConfig config)
        {

            //TODO check if not exist tag or name - return error if exist
            var _socket = _socketList.Find(x => x.tag == tag);
            if (_socket != null)
                return new Tuple<bool, I8Socket>(true,_socket);
            return await CreateSocket(tag,client, config);
        }
        
        public async UniTask<I8Socket> GetSocket(string tag)
        {
            
            if (_socketList.Exists(x => x.tag == tag))
            {
                return _socketList.Find(x => x.tag == tag);
            }
            else
            {
                await UniTask.WaitUntil(() => latestTagCreated == tag );
                return _socketList.Find(x => x.tag == tag);
            }
        }

        
    }
}