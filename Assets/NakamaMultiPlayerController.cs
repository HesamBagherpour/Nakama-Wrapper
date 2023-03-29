using HB.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using HB.Scenes;
using Unity.VisualScripting;
using UnityEngine;

public class NakamaMultiPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public MatchMessageController matchMessageController;
    public GameObject myAvatar;
    // void Awake()
    // {
    //     matchMessageController.OnJoinPlayer +=OnJoinPlayer;
    // }
    //
    // private void OnDestroy()
    // {
    //     matchMessageController.OnJoinPlayer -=OnJoinPlayer;
    // }


    private void OnJoinPlayer(bool isLocalPlayer , bool isPlayerJustJoin,string userId)
    {


        if (isLocalPlayer && !isPlayerJustJoin)
        {
            var myAvatar =  Instantiate(this.myAvatar, NakamaManager.Instance.localPlayer.transform, true);
            var playerRenderer = myAvatar.GetComponent<Renderer>();
            var playerConttroller = myAvatar.GetComponent<NakamaCharacterController>();
            playerConttroller.playerUUID = userId;
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
                playerConttroller.playerUUID = userId;
                playerConttroller.isLocalPlayer = false;
            }
            else
            {
                var myAvatar =  Instantiate(this.myAvatar, NakamaManager.Instance.remotePlayer.transform, true);
                var playerRenderer = myAvatar.GetComponent<Renderer>();
                playerRenderer.material.SetColor("_Color", Color.red);
                var playerConttroller = myAvatar.GetComponent<NakamaCharacterController>();
                playerConttroller.playerUUID = userId;
                playerConttroller.isLocalPlayer = false;
            }
        }
      
       
       
    }

    // Update is called once per frame
 
}
