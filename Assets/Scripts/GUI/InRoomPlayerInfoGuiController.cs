using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InRoomPlayerInfoGuiController : MonoBehaviour
{
    List<Text> playerListText = new List<Text>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            playerListText.Add(transform.GetChild(i).GetComponent<Text>());
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            playerListText[i].text = PhotonNetwork.playerList[i].name;

            if (PhotonNetwork.playerList[i].isMasterClient)
                playerListText[i].color = Color.red;
            else
                playerListText[i].color = Color.white;
        }
    }
}
