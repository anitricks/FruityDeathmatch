using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour
{
    public GameObject StatsUiPrefab;
    private InGamePlayerStatsGuiController statsUIController;

    public int MAX_HP = 100;
    public int curHP { get; private set; }

    void Start()
    {
        CreateStatsUI();
        InitStats();
    }

    private void InitStats()
    {
        curHP = MAX_HP;
        statsUIController.UpdateHpGui(curHP);
    }

    private void CreateStatsUI()
    {
        GameObject statsUI = Instantiate(StatsUiPrefab, new Vector2(0, 0), Quaternion.identity) as GameObject;
        statsUI.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        statsUIController = statsUI.GetComponent<InGamePlayerStatsGuiController>();
        statsUIController.SetFollowTarget(transform);
        statsUIController.SetUsername(photonView.owner.name);
    }

    [PunRPC]
    void ApplyDmg(int dmg)
    {
        curHP -= dmg;

        statsUIController.UpdateHpGui(curHP);

        if (curHP <= 0 && photonView.isMine)
            photonView.RPC("Die", PhotonTargets.All);
    }

    [PunRPC]
    void Die()
    {
        // TODO: Death-Effect Stuff here

        if (photonView.isMine)
            photonView.RPC("Respawn", PhotonTargets.All);
    }


    [PunRPC]
    void Respawn()
    {
        Vector2 spawnPos = Vector2.zero;
        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
            spawnPos = Level.instance.Team_1_Spawn_Point.position;
        else
            spawnPos = Level.instance.Team_2_Spawn_Point.position;

        transform.position = spawnPos;

        // reset stats
        InitStats();
        // TODO: reset ammo and shit
    }

    void OnDestroy()
    {
        if (!photonView.isMine)
            Destroy(statsUIController.gameObject);
    }
}
