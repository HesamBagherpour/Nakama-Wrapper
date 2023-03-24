using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Component;
using HB.NakamaWrapper.Scripts.Runtime.Controller;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using Nakama;
using Newtonsoft.Json;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Modules.Chat
{
    public class ChatConnection : MonoBehaviour
    {
        // Start is called before the first frame update
        private MatchMessageController _matchMessageController;
        private OpCodeGenerator _opCodeGenerator;
        [SerializeField]private ServerClientConfigs serverClientChatConfigs;
        private void Start()
        {
            //ClientConfig clientConfig = new ClientConfig();
            var  uniqueIdentifier = Guid.NewGuid().ToString();
            SessionConfigDevice sessionConfig = new SessionConfigDevice(uniqueIdentifier);
            SocketConfig socketConfig = new SocketConfig();
            RpcConfig rpcConfig = new RpcConfig();
            MatchFactory matchFactory = new MatchFactory();
            ChannelsFactory channelsFactory = new ChannelsFactory();
            Init("aws", "google", "chat", 
                serverClientChatConfigs, sessionConfig, socketConfig,rpcConfig,matchFactory,channelsFactory);
        }

        async void Init<T>(string clientTagName, string sessionTagName, string socketTagName, ServerClientConfigs clientConfig,
            T config, SocketConfig socketConfig, RpcConfig rpcConfig, MatchFactory matchFactory , ChannelsFactory channelsFactory)
            where T : SessionConfig
        {
            var (_, client) = await NakamaManager.Instance.ClientFactory.CreateClint(clientTagName, clientConfig);
            var (_, session) = await client.SessionFactory.CreateSession(sessionTagName,client,config);
            var (_, chatSocket) = await session.SocketFactory.CreateSocket(socketTagName, client, socketConfig);
            
            if (TryGetComponent<SessionConnectionController>(out SessionConnectionController sessionConnectionController))
            {
                sessionConnectionController.Init(client,session,true,2,false);
            }
            
            if (TryGetComponent<MatchConnectionController>(out MatchConnectionController matchConnectionController))
            {
                matchConnectionController.Init(chatSocket);
            }
                     
            if (TryGetComponent<ChannelMessageController>(out ChannelMessageController channelConnectionController))
            {
                channelConnectionController.Init(chatSocket.socket);
            
            }
            if (TryGetComponent<SocketConnectionController>(out SocketConnectionController socketConnectionController))
            {
                socketConnectionController.Init(chatSocket);
                await socketConnectionController.ConnectSocket(chatSocket, session, socketConfig);
            }
            
            
            // if (TryGetComponent<SocketPingPongConnection>(out SocketPingPongConnection socketPingPongConnection))
            // {
            //     await socketPingPongConnection.Init(chatSocket);
            //     chatSocket.socket.ReceivedMatchState += delegate(IMatchState state)
            //     {
            //         socketPingPongConnection.Raise(state);
            //     };
            //
            //     socketPingPongConnection.StartPingPong();
            // }

            #region old

                // if (TryGetComponent<MatchMessageControllerOld>(out MatchMessageControllerOld messageConnectionController))
                // {
                //     messageConnectionController.Init(chatSocket);
                //     chatSocket.socket.ReceivedMatchState += delegate(IMatchState state)
                //     {
                //         messageConnectionController.Raise(state);
                //     };
                //
                // }

            #endregion
            
            var (_, match) = await matchFactory.CreateMatch("chat", client, session, rpcConfig);
            Debug.Log(match.data.matchId);
            
            if (TryGetComponent<MatchMessageController>(out MatchMessageController _matchMessageController))
            {
                _matchMessageController.Init(chatSocket,session.Session.Username,match.data.matchId,session.Session.UserId);
                this._matchMessageController = _matchMessageController;
            }
            
            await matchConnectionController.ConnectMatch(match.data.matchId);
    
            if (TryGetComponent<OpCodeGenerator>(out OpCodeGenerator opCodeGenerator))
            {
                _opCodeGenerator = opCodeGenerator;
                _opCodeGenerator.Init(session.Session.Username,match.data.matchId);
                matchConnectionController.OnMatchConnect?.Invoke(matchConnectionController._matchId);
            }
            
            
            // Debug.unityLogger.Log(await client.client.RpcAsync(session.Session,"test"));
            //
            ChannelConfig channelConfig = new ChannelConfig(true ,true,"Chat",ChannelType.Group);
            var (_, channel) = await channelsFactory.CreateChannel(session.Session.UserId,chatSocket,channelConfig);
            
      
            
            // var content = new Dictionary<string, string> {{"hello", "world"}};
            // await channelConnectionController.SendMessages(channel.Id, content);
            //SendChannelMessage(channel.Id, channelConnectionController);
           
            
            // var opCode = 300;
            // var newState = new Dictionary<string, string> {{"test", "test"}}.ToJson();
            // await chatSocket.socket.SendMatchStateAsync(match.data.matchId,opCode,newState);


        }




        private async void SendChannelMessage(string channelId , ChannelMessageController channelConnectionController)
        {
            
            var content = new Dictionary<string, string> {{"hello", "world"}};
            await channelConnectionController.SendMessages(channelId, content);
        }
        
        
        
        public void SendMatchState(Vector3 pos)
        {
            string opCodeKey = "move2";
            MultiPlayerMessage<MoveStateModelNew> packet = new MultiPlayerMessage<MoveStateModelNew>();
            packet.message = new MoveStateModelNew(pos.x,pos.y,pos.z,pos);
            packet.uuid = _opCodeGenerator.getOpCode(0,opCodeKey).Uuid;
            _matchMessageController.SendMatchState(_opCodeGenerator.getOpCode(0, opCodeKey).OpCode,
                JsonConvert.SerializeObject(packet, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    })).Forget();
        }


        // private void Update()
        // {
        //     if (Input.GetButtonDown("Jump"))
        //     {
        //         Vector3 pos = new Vector3(5, 5, 5);
        //         SendMatchState(pos);
        //     }
        // }
    }
    
}
