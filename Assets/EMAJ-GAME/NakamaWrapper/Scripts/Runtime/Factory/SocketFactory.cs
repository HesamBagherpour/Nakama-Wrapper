using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory
{
    public class SocketFactory
    {
        private List<EM_Socket> _socketList = new List<EM_Socket>();
        public Action<EM_Socket> OnCreateSocket;
        private string latestTagCreated;
        public EM_Socket socket;
        public SocketFactory()
        {
            _socketList = new List<EM_Socket>();
        }
        public async UniTask<Tuple<bool, EM_Socket>> CreateSocket(string tag,EM_Client client,SocketConfig config)
        {
            
            //TODO check if not exist tag or name - return error if exist
            
            socket = _socketList.Find(x => x.tag == tag);
            if (socket != null)
                return new Tuple<bool, EM_Socket>(true,socket);
            
            socket = new EM_Socket(tag,client,config);
            // this socket already exist with same name 
            
            // if(matchMessageController == null)
            //     socket = new I8Socket(tag,client,config);
            // else
            //     socket = new I8Socket(tag,client,config ,matchMessageController,matchConnectionController);
    
            socket = await socket.Init();
            _socketList.Add(socket);
            OnCreateSocket?.Invoke(socket);
            latestTagCreated = tag;
            return new Tuple<bool, EM_Socket>(true, socket);
        }

        
        
        public async UniTask<Tuple<bool, EM_Socket>> CreateOrGetSocket(string tag,EM_Client client ,SocketConfig config)
        {

            //TODO check if not exist tag or name - return error if exist
            var _socket = _socketList.Find(x => x.tag == tag);
            if (_socket != null)
                return new Tuple<bool, EM_Socket>(true,_socket);
            return await CreateSocket(tag,client, config);
        }
        
        public async UniTask<EM_Socket> GetSocket(string tag)
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