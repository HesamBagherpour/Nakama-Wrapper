using HB.NakamaWrapper.Scripts.Runtime.Component;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using HB.NakamaWrapper.Scripts.Runtime.Modules.Chat;
using Nakama.Snippets;
using UnityEngine;
using NakamaManager = HB.NakamaWrapper.Scripts.Runtime.Manager.NakamaManager;

namespace HB.Scenes
{
    public class NakamaCharacterController : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField]private ChatConnection chatConnection;
        public string playerUUID;
        private MatchMessageController _matchMessageController;
        private float playerSpeed = 2.0f;
        public Vector3 oldPos;
        public bool isLocalPlayer;
        private void Awake()
        {
            _controller = gameObject.AddComponent<CharacterController>();
            chatConnection = FindObjectOfType<ChatConnection>(true);
            _matchMessageController = FindObjectOfType<MatchMessageController>();
            if (isLocalPlayer)
            {
                chatConnection = FindObjectOfType<ChatConnection>(true);
            }
            _matchMessageController.OnPlayerPosition +=OnPlayerPosition;
            
        }

        private void OnPlayerPosition(MoveStateModelNew obj,string uuid)
        {
            if (NakamaManager.Instance.userId == uuid)
                return;
            if (playerUUID == uuid)
            {
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
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
                    chatConnection.SendMatchState(position);
                    oldPos = transform.position;
                }
                
            }

        }
    }
    
}
