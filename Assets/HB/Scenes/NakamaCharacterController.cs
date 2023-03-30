using HB.NakamaWrapper.Scripts.Runtime.Modules.Chat;
using UnityEngine;

namespace HB.Scenes
{
    public class NakamaCharacterController : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField]private ChatConnection chatConnection;
        private float playerSpeed = 2.0f;
        public Vector3 oldPos;
        public bool isLocalPlayer;
        private void Awake()
        {
            _controller = gameObject.AddComponent<CharacterController>();
            chatConnection = FindObjectOfType<ChatConnection>(true);
            if (isLocalPlayer)
            {
                chatConnection = FindObjectOfType<ChatConnection>(true);
            }
            
        }

        private void Update()
        {
            // if (isLocalPlayer)
            // {
            //     Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //     _controller.Move(move * Time.deltaTime * playerSpeed);
            //
            //     if (transform.position != oldPos)
            //     {
            //
            //         var transform1 = transform;
            //         var position = transform1.position;
            //         chatConnection.SendMatchState(position);
            //
            //         oldPos = transform.position;
            //     }
            //     
            // }

        }
    }
    
}
