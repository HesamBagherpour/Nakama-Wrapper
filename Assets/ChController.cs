using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Components;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Nakama.TinyJson;
using Newtonsoft.Json;
using UnityEngine;

public class ChController : MonoBehaviour
{
    // Start is called before the first frame update

    public string matchId;
    public string matchTagName;
    public string sessionTagName;
    private OpCodeGenerator _opCodeGenerator;
    private bool _isOpCodeRigesterd;

    async Task Start()
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        _opCodeGenerator = GetComponent<OpCodeGenerator>();
        _opCodeGenerator.OnReceiveOpCodeMessage +=OnReceiveOpCodeMessage;
        _opCodeGenerator.Onconnected +=Onconnected;
        var clint = await NakamaManager.Instance.ClientFactory.GetClintAsync("aws");
        Debug.Log("clint : " + clint);

        //_opCodeGenerator.Init(_opCodeGenerator._matchConnectionController,_opCodeGenerator._matchConnectionController.matchMessageController.MatchOpCodeController);
    }

    private void ONGetCallBack(I8Client obj)
    {
        
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
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Vector3 pos = new Vector3(5, 5, 5);
            SendMatchState(pos);
        }
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
