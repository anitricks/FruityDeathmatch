using UnityEngine;
using System.Collections;
using Photon;

public class Network : Photon.MonoBehaviour
{
    public bool AutoConnect = false;
    public byte Version = 1;
    private bool ConnectInUpdate = false;
    private bool connectFailed = false;

    public LobbyRoomList roomList;


    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
    }

    public virtual void Update()
    {
        // get room list
        // in lobby 
        if (GameManager.instance.gameState == GameManager.GameState.Lobby)
        {
            if (roomList == null)
                roomList = GameObject.FindWithTag("RoomList").GetComponent<LobbyRoomList>();

            if (PhotonNetwork.GetRoomList().Length != 0)
                roomList.ShowRoom(PhotonNetwork.GetRoomList());
            else
                roomList.NoRoom();
        }
    }

    public void Connect()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = false;

        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)

            PhotonNetwork.ConnectToBestCloudServer("1.0v");
        }

        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + Application.loadedLevel);

         }



        // generate a name for this player, if none is assigned yet
        /*if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
        }
        */
        // if you wanted more debug out, turn this on:
        // PhotonNetwork.logLevel = NetworkLogLevel.Full;
    }

    // to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!
    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        //Debug.Log(PhotonNetwork.GetPing());
        //PhotonNetwork.JoinLobby();
        //PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
    }

    public void JoinRoom(string roomName, string playerName)
    {
        PhotonNetwork.playerName = playerName;
        PhotonNetwork.JoinRoom(roomName);
    }
    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");

        Application.LoadLevel("3_Room");
    }

    public void InitGameData()
    {
        if (PhotonNetwork.isMasterClient)
            SetUpMasterGameData();
        //else
        //    FetchGameDataFromMaster();
    }

    void SetUpMasterGameData()
    {
        Debug.Log("setup master data");
        //MasterClient.instance.StartGame();
    }

    public void CreateRoom(string roomName, string playerName)
    {
        PhotonNetwork.playerName = playerName;

        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 6 }, null);

        //PhotonNetwork.room
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");


        Application.LoadLevel("3_Room");

        //PhotonNetwork.LoadLevel(SceneNameGame);
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        this.connectFailed = true;
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);

    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("player connected");
    }

    /// <summary>
    /// game start
    /// </summary>
    public void InitGame()
    {
        CreatePlayer();

        if (PhotonNetwork.isMasterClient)
        {
            InitLevel();
        }
    }

    private void InitLevel()
    {
        // TODO : format level name
        //PhotonNetwork.Instantiate("Prefab/Level/Level_0_Terrain", Vector3.zero, Quaternion.identity, 0);
        //PhotonNetwork.InstantiateSceneObject("Prefab/Level/Level_0_Terrain", Vector3.zero, Quaternion.identity, 0);

        // create sun
        //GameObject sun = PhotonNetwork.Instantiate("Prefab/Environment/Sun", new Vector3(250, 300, 250), Quaternion.identity, 0);
    }


    private void CreatePlayer()
    {
        //Vector3[] pos = new Vector3[3] { new Vector3(200, 0, 200), new Vector3(250, 0, 250), new Vector3(225, 0, 225) };

        GameObject player = PhotonNetwork.Instantiate("Prefab/player", new Vector2(0, 2), Quaternion.identity, 0) as GameObject;

        //GameObject player = PhotonNetwork.Instantiate("Prefab/Player/Player", new Vector3(250, 0, 250), Quaternion.identity, 0) as GameObject;

        player.name = "localplayer";
        player.tag = "Player";

        FollowPlayer.instance.SetTarget(player.transform);

        Debug.Log("create player");
    }

}
