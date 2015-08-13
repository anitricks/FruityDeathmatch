using UnityEngine;
using ExitGames.Client.Photon;

public class MasterClient : MonoBehaviour
{

    public static MasterClient instance;

    public double StartTime;

    private const string StartTimeKey = "st";       // the name of our "start time" custom property.

    bool gameStarted;
    bool startTimeNeedsSyncWithServer;

    void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        gameStarted = true;
        SetStartGameTimer();
    }

    public void SetStartGameTimer()
    {
        // havn't gotten the time from server
        if (PhotonNetwork.time < 0.0001f)
        {
            startTimeNeedsSyncWithServer = true;
            return;
        }

        // successfully got the time from server
        startTimeNeedsSyncWithServer = false;

        // set custom prop for start time
        Hashtable startTimeData = new Hashtable();
        startTimeData[StartTimeKey] = PhotonNetwork.time;
        PhotonNetwork.room.SetCustomProperties(startTimeData);

        //GameObject.FindWithTag("TimeManager").GetComponent<TimeManager>().SetStartTime();

        Debug.Log(PhotonNetwork.room.customProperties);
    }


    void Update()
    {
        if (startTimeNeedsSyncWithServer)
            SetStartGameTimer();

    }






}
