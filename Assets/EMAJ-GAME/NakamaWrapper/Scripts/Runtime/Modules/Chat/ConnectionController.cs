namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Modules.Chat
{
    // public class ConnectionController:I8SocketConnection
    // {
    //     public async void Init<T>(string clientTagName , string sessionTagName ,string socketTagName,ClientConfig clientConfig,T config,SocketConfig socketConfig) where T : SessionConfig
    //     {
    //             var  (_ , client ) = await NakamaManager.Instance.clientController.CreateOrGetClint(clientTagName,clientConfig);
    //             var  (_ , session ) = await client.sessionController.CreateSession(sessionTagName,client , config);
    //             var  (_ , chatSocket ) = await  session.socketController.CreateSocket(socketTagName, client, session.Session,socketConfig);
    //             chatSocket.socket.Connected += SocketOnConnected;
    //             chatSocket.socket.Closed +=SocketOnClosed;
    //             chatSocket.socket.ReceivedMatchState += OnReceivedMatchState;
    //     }
    //
    //    public event Action<IMatchState> OnReceivedMatchState;
    //
    //    public event Action<IMatchPresenceEvent> OnReceivedMatchPresence;
    //
    //    public event Action SocketOnClosed;
    //
    //    public event Action SocketOnConnected;
    // }
}