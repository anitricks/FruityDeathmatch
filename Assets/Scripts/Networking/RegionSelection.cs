using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;

public class RegionServer
{
    public string name;
    public string address;
    public int ping;
    public bool pingDone = false;
}

public class RegionSelection : Photon.MonoBehaviour
{
    string[] regionNames = new string[5] { "Asia", "US", "EU", "Japan", "Australia" };
    string[] serverAddress = new string[5] { "119.81.90.46", "184.173.132.27", "37.58.117.146", "157.7.168.43", "168.1.77.4" };

    List<RegionServer> cloudServers = new List<RegionServer>();

    bool serverIsInit = false;
    bool pingFinishAll = true;

    public List<Button> serverButton = new List<Button>();
    public List<Text> serverName = new List<Text>();
    public List<Text> ping = new List<Text>();

    private float timer = 1.5f;
    private float timePassed = 0;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);

            serverButton.Add(t.GetComponent<Button>());

            serverName.Add(t.GetChild(0).GetComponent<Text>());
            //t.GetChild(0).GetComponent<Text>().text = regionNames[i];            
            ping.Add(t.GetChild(1).GetComponent<Text>());
        }

        InitServer();
    }

    void InitServer()
    {
        for (int i = 0; i < regionNames.Length; i++)
        {
            RegionServer region = new RegionServer();
            region.name = regionNames[i];
            region.address = serverAddress[i];
            region.ping = -1;
            cloudServers.Add(region);
        }

        serverIsInit = true;

        PingAll();
        PingGUI();
    }

    void FixedUpdate()
    {
        if (serverIsInit)
        {
            timePassed += Time.fixedDeltaTime;

            if (timePassed >= timer)
            {
                timePassed -= timer;

                PingAll();
                PingGUI();
            }
        }
    }

    void PingAll()
    {
        pingFinishAll = false;

        int index = 0;
        StartCoroutine(PingAddress(index));

    }

    void PingGUI()
    {
        // update ping gui
        for (int i = 0; i < cloudServers.Count; i++)
        {
            if (cloudServers[i].pingDone)
            {
                serverName[i].text = cloudServers[i].name;
                ping[i].text = cloudServers[i].ping.ToString() + " ms";
            }
        }
    }

    IEnumerator PingAddress(int index)
    {
        //Debug.Log(index);

        Ping ping = new Ping(cloudServers[index].address);
        while (!ping.isDone) { yield return null; }

        cloudServers[index].ping = ping.time;
        cloudServers[index].pingDone = true;

        index++;
        if (index == cloudServers.Count)
        {
            pingFinishAll = true;
            index = 0;
        }

        StartCoroutine(PingAddress(index));

        //Debug.Log(server.name + " " + server.ping);
    }

    public void SelectRegion(int index)
    {
        PhotonNetwork.PhotonServerSettings.ServerAddress = cloudServers[index].address;

        CloudRegionCode region = new CloudRegionCode();

        switch (index)
        {
            case 0:
                region = CloudRegionCode.asia;
                break;

            case 1:
                region = CloudRegionCode.us;
                break;

            case 2:
                region = CloudRegionCode.eu;
                break;

            case 3:
                region = CloudRegionCode.jp;
                break;

            case 4:
                region = CloudRegionCode.au;
                break;
        }

        PhotonNetwork.OverrideBestCloudServer(region);

        GameManager.instance.network.Connect();

        Application.LoadLevel("2_Lobby");

    }
}
