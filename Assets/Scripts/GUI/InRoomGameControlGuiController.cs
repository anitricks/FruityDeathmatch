using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InRoomGameControlGuiController : MonoBehaviour
{
    public GameObject ReadyBtnObj;
    public GameObject StartGameBtnObj;

    void Start()
    {
        if (PhotonNetwork.player.isMasterClient)
            ReadyBtnObj.SetActive(false);
        else
            StartGameBtnObj.SetActive(false);
    }

    public void OnClickStart()
    {
        GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.All);
    }

    [PunRPC]
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}
