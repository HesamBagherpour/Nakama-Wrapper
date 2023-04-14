using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Components;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Manager;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Newtonsoft.Json;
using UnityEngine;

namespace Emaj_Game.UnitySample.Scripts
{
    public class ChController : MonoBehaviour
    {
        // Start is called before the first frame update

        public string matchId;
        public string matchTagName;
        public string sessionTagName;
        public string socketTagName;
        public string clintTagName;
        private OpCodeGenerator _opCodeGenerator;
        private bool _isOpCodeRigesterd;

        private ISocket mySocket;

        public MatchMessageController matchMessageController;

        async Task Start()
        {

            Debug.Log("Character Controller Start ");
            _opCodeGenerator = GetComponent<OpCodeGenerator>();
            _opCodeGenerator.OnReceiveOpCodeMessage +=OnReceiveOpCodeMessage;
            _opCodeGenerator.Onconnected +=Onconnected;
            var clint = await NakamaManager.Instance.ClientFactory.GetClintAsync("aws");
            clintTagName = clint.tag;
            var session = await clint.SessionFactory.GetSession("google");
            sessionTagName = session.tag;

            var socket = await session.SocketFactory.GetSocket("chat");
            socketTagName = socket.tag;
            mySocket = socket.socket;
        
            var match = await socket.MatchFactory.GetMatch("chat");
            matchId = match.matchId;
            matchTagName = match.tag;

            var matchMessage = match.matchMessageController;
            var matchConnection = match.matchConnectionController;
            _opCodeGenerator.Init(
                matchConnection,
                matchMessage.matchOpCodeController);
        }
    

        private void Onconnected()
        {
            _isOpCodeRigesterd = true;
        }
        private void OnReceiveOpCodeMessage(long opCode, string key, string uuid, IMatchState state)
        {
            var packet =JsonConvert.DeserializeObject<MultiPlayerMessage<MoveStateModelNew>>(Encoding.UTF8.GetString(state.State)) ;
            if (packet != null && packet.needLastStateUserId != null)
            {
                // send Last State if there is not any state 
                // or new player jut Join
            }
        
            Debug.Log(packet.message.pos);
      
        }
        private async Task Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Vector3 pos = new Vector3(5, 5, 5);
                SendMatchState(pos);
            }

            // if (Input.GetButtonDown("Fire1"))
            // {
            //     if (mySocket.IsConnected)
            //         await mySocket.CloseAsync();
            // }
        }
        private void SendMatchState(Vector3 pos)
        {
            if (_isOpCodeRigesterd)
            {
                string opCodeKey = "ch";
                MultiPlayerMessage<MoveStateModelNew> packet = new MultiPlayerMessage<MoveStateModelNew>();
                packet.message = new MoveStateModelNew(pos.x,pos.y,pos.z,pos);
                packet.uuid = _opCodeGenerator.GetOpCode(0,opCodeKey).Uuid;
                _opCodeGenerator.SendMatchState(_opCodeGenerator.GetOpCode(0, opCodeKey).OpCode,
                    JsonConvert.SerializeObject(packet, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        })).Forget();
            }

        }
    }
}
