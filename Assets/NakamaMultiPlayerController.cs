using System;
using System.Collections;
using System.Collections.Generic;
using HB.NakamaWrapper.Scripts.Runtime.Component;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using UnityEngine;

public class NakamaMultiPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public MatchMessageController matchMessageController;
    public GameObject avatar;
    void Awake()
    {
        matchMessageController.OnJoinPlayer +=OnJoinPlayer;
    }

    private void OnDestroy()
    {
        matchMessageController.OnJoinPlayer -=OnJoinPlayer;
    }


    private void OnJoinPlayer()
    {

       var myAvatar =  Instantiate(avatar, NakamaManager.Instance.localPlayer.transform, true);
    }

    // Update is called once per frame
 
}
