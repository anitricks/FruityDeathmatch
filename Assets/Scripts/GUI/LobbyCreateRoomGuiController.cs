using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyCreateRoomGuiController : MonoBehaviour {

    public Text roomnameText;

    public void OnClickCreateRoom()
    {
        GameManager.instance.network.CreateRoom(roomnameText.text, UserInfo.instance.username);
    }
}
