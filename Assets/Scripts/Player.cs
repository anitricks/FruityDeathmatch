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
        InitStats();
        CreateStatsUI();
    }

    private void InitStats()
    {
        curHP = MAX_HP;
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
    }

    void OnDestroy()
    {
        Destroy(statsUIController.gameObject);
    }
}
