using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Unity.VisualScripting;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class SocketFactory
    {
        private List<I8Socket> _socketList;

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
                return new Tuple<bool, I8Socket>(false,null); // this socket already exist with same name 
            socket = new I8Socket(tag,client,config);
            socket = await socket.Init();
            //await socket.socket.ConnectAsync(session,config.AppearOnline,config.ConnectionTimeout);
            
            
            return new Tuple<bool, I8Socket>(true, socket);
        }
        
        
        public async UniTask<Tuple<bool, I8Socket>> CreateOrGetSocket(string tag,I8Session session ,SocketConfig config)
        {
            return new Tuple<bool, I8Socket>(true, null);
        }
        
        public I8Socket GetSocket(string tag)
        {
            return _socketList.Find(x => x.tag == tag);
        }
        
        public void openSocket()
        {
            I8Socket socket;
        }
        
    }
}