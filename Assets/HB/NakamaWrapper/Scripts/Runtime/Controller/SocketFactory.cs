using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class SocketFactory
    {
        private List<HSocket> _socketList;

        public SocketFactory()
        {
            _socketList = new List<HSocket>();
        }
        public async UniTask<Tuple<bool, HSocket>> CreateSocket(string tag,HClient client,SocketConfig config)
        {
            
            //TODO check if not exist tag or name - return error if exist
            HSocket socket;
            socket = _socketList.Find(x => x.tag == tag);
            if (socket != null)
                return new Tuple<bool, HSocket>(false,null); // this socket already exist with same name 
            socket = new HSocket(tag,client,config);
            socket = await socket.Init();
            //await socket.socket.ConnectAsync(session,config.AppearOnline,config.ConnectionTimeout);
            
            
            return new Tuple<bool, HSocket>(true, socket);
        }
        
        
        public async UniTask<Tuple<bool, HSocket>> CreateOrGetSocket(string tag,HSession session ,SocketConfig config)
        {
            return new Tuple<bool, HSocket>(true, null);
        }
        
        public HSocket GetSocket(string tag)
        {
            return _socketList.Find(x => x.tag == tag);
        }
        
        public void openSocket()
        {
            HSocket socket;
        }
        
    }
}