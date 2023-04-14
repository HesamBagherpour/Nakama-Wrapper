using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Socket
{
    public class SocketConnectionController : MonoBehaviour
    {
        private EM_Socket _emSocket;
        private EM_Client _emClient;
        private EM_Session _emSession;
        public Action<EM_Socket> OnConnectSocket;
        public Action<EM_Socket> OnDisconectSocket;
        [SerializeField]public List<string> playerList;

        public void Init(EM_Client clint , EM_Session session,EM_Socket socket)
        {
            _emSocket = socket;
            _emClient = clint;
            _emSession = session;
            socket.socket.Connected+=SocketOnConnected;
            socket.socket.Closed+=SocketOnClosed;
            socket.socket.ReceivedError +=SocketOnReceivedError;
            socket.socket.ReceivedStatusPresence +=SocketOnReceivedStatusPresence;
            socket.socket.ReceivedMatchPresence +=SocketOnReceivedMatchPresence;
        }
        private void SocketOnReceivedMatchPresence(IMatchPresenceEvent obj)
        {
            foreach (var user in obj.Joins)
            {
                if(user.UserId == _emSession.Session.UserId)
                    return;
                AddFriend(user.UserId);
            }
            foreach (var user in obj.Leaves)
            {
    
                removeFriend(user.UserId);
            }
        }
        private async void AddFriend(string userId)
        {
            playerList.Add(userId);
            var ids = new[] {userId};
            await _emClient.client.AddFriendsAsync(_emSession.Session, ids, null);
        }
        private async void removeFriend(string userId)
        {
            var ids = new[] {userId};
            playerList.Remove(userId);
            await _emClient.client.DeleteFriendsAsync(_emSession.Session, ids, null);
        }
        private void SocketOnReceivedStatusPresence(IStatusPresenceEvent presenceEvent )
        {
            Debug.Log(presenceEvent);
            // foreach (var joined in presenceEvent.Joins)
            // {
            //
            //     Console.WriteLine("User id '{0}' status joined '{1}'", joined.UserId, joined.Status);
            // }
            // foreach (var left in presenceEvent.Leaves)
            // {
            //     Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaa" + left.UserId);
            //     Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaa" + left.Username);
            //     Console.WriteLine("User id '{0}' status left '{1}'", left.UserId, left.Status);
            // }
        }

        public async UniTask ConnectSocket(EM_Socket socket,EM_Session session,SocketConfig config)
        {
            
            await socket.socket.ConnectAsync(session.Session,config.AppearOnline,config.ConnectionTimeout);
            await socket.socket.UpdateStatusAsync("hesam");

        }
        private void SocketOnReceivedError(Exception obj)
        {
            
        }
        private void SocketOnClosed()
        { 
            playerList.Clear();
        }
        private void SocketOnConnected() 
        {
            OnConnectSocket?.Invoke(_emSocket);
        }
        
    }
    
}