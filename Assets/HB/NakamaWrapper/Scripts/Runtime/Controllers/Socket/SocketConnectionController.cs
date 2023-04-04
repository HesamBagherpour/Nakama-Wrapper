using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Socket
{
    public class SocketConnectionController : MonoBehaviour
    {
        private I8Socket _i8Socket;
        public Action<I8Socket> OnCreateSocket;
        public void Init(I8Socket socket)
        {
            _i8Socket = socket;
            socket.socket.Connected +=SocketOnConnected;
            socket.socket.Closed +=SocketOnClosed;
            socket.socket.ReceivedError +=SocketOnReceivedError;
        }
        public async UniTask ConnectSocket(I8Socket socket,I8Session session,SocketConfig config)
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