using System;
using System.Collections;
using System.Collections.Generic;
using HB.NakamaWrapper.Scripts.Runtime.Component;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using HB.Scenes;
using Unity.VisualScripting;
using UnityEngine;

public class NakamaMultiPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public MatchMessageController matchMessageController;
    public GameObject myAvatar;
    void Awake()
    {
        matchMessageController.OnJoinPlayer +=OnJoinPlayer;
    }

    private void OnDestroy()
    {
        matchMessageController.OnJoinPlayer -=OnJoinPlayer;
    }


    private void OnJoinPlayer(bool isLocalPlayer , bool isPlayerJustJoin)
    {


        if (isLocalPlayer && !isPlayerJustJoin)
        {
            var myAvatar =  Instantiate(this.myAvatar, NakamaManager.Instance.localPlayer.transform, true);
            var playerRenderer = myAvatar.GetComponent<Renderer>();
            var playerConttroller = myAvatar.GetComponent<NakamaCharacterController>();
            playerConttroller.isLocalPlayer = true;
            playerRenderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            if (!isLocalPlayer && isPlayerJustJoin)
            {
                var myAvatar =  Instantiate(this.myAvatar, NakamaManager.Instance.remotePlayer.transform, true);
                var playerRenderer = myAvatar.GetComponent<Renderer>();
                playerRenderer.material.SetColor("_Color", Color.yellow);
                var playerConttroller = myAvatar.GetComponent<NakamaCharacterController>();
                playerConttroller.isLocalPlayer = false;
            }
            else
            {
                var myAvatar =  Instantiate(this.myAvatar, NakamaManager.Instance.remotePlayer.transform, true);
                var playerRenderer = myAvatar.GetComponent<Renderer>();
                playerRenderer.material.SetColor("_Color", Color.red);
                var playerConttroller = myAvatar.GetComponent<NakamaCharacterController>();
                playerConttroller.isLocalPlayer = false;
            }
        }
      
       
       
    }

    // Update is called once per frame
 
}
