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
    public class NakamaCharacterController : MonoBehaviour
    {
        private CharacterController _controller;
        private float playerSpeed = 2.0f;
        public Vector3 oldPos;
        public bool isLocalPlayer;
        public string playerUserId;

        public string matchId;
        public string matchTagName;
        public string sessionTagName;
        public string socketTagName;
        public string clintTagName;
        private OpCodeGenerator _opCodeGenerator;
        private bool _isOpCodeRigesterd;


        public MatchMessageController matchMessageController;

        private void Awake()
        {
            _controller = gameObject.AddComponent<CharacterController>();
            // chatConnection = FindObjectOfType<ChatConnection>(true);
            // if (isLocalPlayer)
            // {
            //     chatConnection = FindObjectOfType<ChatConnection>(true);
            // }

        }

        async Task Start()
        {

            Debug.Log("Character Controller Start ");
            _opCodeGenerator = GetComponent<OpCodeGenerator>();
            _opCodeGenerator.OnReceiveOpCodeMessage += OnReceiveOpCodeMessage;
            _opCodeGenerator.Onconnected += Onconnected;
            var clint = await NakamaManager.Instance.ClientFactory.GetClintAsync("aws");
            clintTagName = clint.tag;
            var session = await clint.SessionFactory.GetSession("google");
            sessionTagName = session.tag;

            var socket = await session.SocketFactory.GetSocket("chat");
            socketTagName = socket.tag;

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
            var packet =
                JsonConvert.DeserializeObject<MultiPlayerMessage<MoveStateModelNew>>(
                    Encoding.UTF8.GetString(state.State));
            if (packet != null && packet.needLastStateUserId != null)
            {
                // send Last State if there is not any state 
                // or new player jut Join
            }

            Debug.Log(packet.message.pos);
        }
        private void SendMatchState(Vector3 pos)
        {
            if (_isOpCodeRigesterd)
            {
                string opCodeKey = "move";
                MultiPlayerMessage<MoveStateModelNew> packet = new MultiPlayerMessage<MoveStateModelNew>();
                packet.message = new MoveStateModelNew(pos.x, pos.y, pos.z, pos);
                packet.uuid = _opCodeGenerator.GetOpCode(0, opCodeKey).Uuid;
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
        private void Update()
        {
            if (isLocalPlayer)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                _controller.Move(move * Time.deltaTime * playerSpeed);
            
                if (transform.position != oldPos)
                {
            
                    var transform1 = transform;
                    var position = transform1.position;
                    SendMatchState(position);
            
                    oldPos = transform.position;
                }
            }
        }

    }

}
