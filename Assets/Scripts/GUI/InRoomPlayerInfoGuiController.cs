using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InRoomPlayerInfoGuiController : Photon.PunBehaviour
{
    public List<Text> teamBlueListText;
    public List<Text> teamRedListText;

    List<PhotonPlayer> teamBlue;
    List<PhotonPlayer> teamRed;

    void Start()
    {
        UpdateRoomPlayerList();
    }

    void UpdateRoomPlayerList()
    {
        // reset
        teamBlue = new List<PhotonPlayer>();
        teamRed = new List<PhotonPlayer>();
        foreach (var text in teamBlueListText)
        {
            text.color = Color.white;
            text.text = "empty";
        }
        foreach (var text in teamRedListText)
        {
            text.color = Color.white;
            text.text = "empty";
        }

        // get blue team player
        foreach (var player in PhotonNetwork.playerList)
        {
            if (player.GetTeam() == PunTeams.Team.blue)
                teamBlue.Add(player);
            else
                teamRed.Add(player);
        }

        // get red team player
        for (int i = 0; i < teamBlue.Count; i++)
        {
            if (teamBlue[i].isMasterClient)
                teamBlueListText[i].color = Color.red;
            else
                teamBlueListText[i].color = Color.white;

            teamBlueListText[i].text = teamBlue[i].name;
        }

        for (int i = 0; i < teamRed.Count; i++)
        {
            if (teamRed[i].isMasterClient)
                teamRedListText[i].color = Color.red;
            else
                teamRedListText[i].color = Color.white;

            teamRedListText[i].text = teamRed[i].name;
        }
    }

    public void SwitchTeam(int teamIndex)
    {
        if (teamIndex == 1)
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
        else
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);

        UpdateRoomPlayerList();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        UpdateRoomPlayerList();
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        UpdateRoomPlayerList();
    }
    // TODO: update list when leave,join and more shitty situation


}
