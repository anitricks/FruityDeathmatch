using UnityEngine;
using System.Collections;
using Photon;
using ExitGames.Client.Photon;

public class GameManager : Photon.MonoBehaviour
{
    public enum GameState
    {
        Menu,
        RegionSelection,
        Lobby,
        Room,
        Game
    }

    public static GameManager instance { get; private set; }
    public GameState gameState { get; set; }

    public Network network { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(gameObject);

        instance = this;

        gameState = GameState.Menu;

        network = GetComponent<Network>();
    }

    //
    //
    // Menu Accessor

    public void StartGame(bool single = true)
    {
        PhotonNetwork.offlineMode = single;

        LoadNextLevel();
    }

    // Menu Accessor
    // 
    // 
    void OnLevelWasLoaded(int level)
    {
        // Lobby Scene
        if (Application.loadedLevel == 2)
        {
            gameState = GameState.Lobby;

            Debug.Log("lobby state");
        }

        else if (Application.loadedLevel == 3)
        {
            gameState = GameState.Room;

            Debug.Log("room state");
        }

        // Game Scene
        else if (Application.loadedLevel == 4)
        {
            Debug.Log("game state");

            gameState = GameState.Game;

            // single player
            if (PhotonNetwork.offlineMode)
            {
                // instantiate directely

                Debug.Log("offline");
            }
            // multi player
            else
            {
                Debug.Log("online");

                network.InitGame();
                network.InitGameData();
            }

            // **************
            // **************
            // move to a method or something

            // Create local camera
            //Instantiate(Resources.Load("Prefab/Camera/Cam", typeof(GameObject)));

            // Create Joystick
            //#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
            //            Instantiate(Resources.Load("Prefab/Utils/Joystick", typeof(GameObject)));
            //#endif
        }
    }

    public void LoadNextLevel()
    {
        int nextLevel = Application.loadedLevel;
        nextLevel++;

        if (PhotonNetwork.offlineMode)
            Application.LoadLevel(nextLevel);
        else
            PhotonNetwork.LoadLevel(nextLevel);
    }
}
