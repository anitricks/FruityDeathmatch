using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyRoomList : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>();
    public List<Text> roomName = new List<Text>();

    public RoomInfo[] roomInfo;

    //public LobbyGuiController guiController;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            rooms.Add(transform.GetChild(i).gameObject);
            rooms[i].SetActive(false);

            roomName.Add(rooms[i].transform.GetChild(0).GetComponent<Text>());
        }
    }

    public void ShowRoom(RoomInfo[] roomInfo)
    {
        this.roomInfo = roomInfo;

        for (int i = 0; i < roomInfo.Length; i++)
        {
            rooms[i].SetActive(true);
            roomName[i].text = roomInfo[i].name;
        }
    }

    public void NoRoom()
    {
        for (int i = 0; i < transform.childCount; i++)
            rooms[i].SetActive(false);
    }

    public void ClickRoom(int index)
    {
        //guiController.JoinRoom(roomName[index].text);
        GameManager.instance.network.JoinRoom(roomName[index].text, UserInfo.instance.username);
    }

}
