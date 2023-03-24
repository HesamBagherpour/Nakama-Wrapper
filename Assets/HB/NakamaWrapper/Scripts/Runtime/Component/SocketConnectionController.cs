using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class SocketConnectionController : MonoBehaviour
    {
        private HSocket _hSocket;
        public void Init(HSocket socket)
        {
            _hSocket = socket;
            socket.socket.Connected +=SocketOnConnected;
            socket.socket.Closed +=SocketOnClosed;
            socket.socket.ReceivedError +=SocketOnReceivedError;
        }
        public async UniTask ConnectSocket(HSocket socket,HSession session,SocketConfig config)
        {

            await socket.socket.ConnectAsync(session.Session,config.AppearOnline,config.ConnectionTimeout);

        }
        private void SocketOnReceivedError(Exception obj)
        {
            Debug.Log("Socket is  SocketOnReceivedError " + obj);
        }
        private void SocketOnClosed()
        { 
            Debug.Log("Socket is  SocketOnClosed ");
        }
        private void SocketOnConnected() 
        {
            Debug.Log("Socket is  Connected ");
     
        }
        
    }
    
}