using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGamePlayerStatsGuiController : Photon.MonoBehaviour
{
    public Image hbBarImg;
    public Text usernameText;

    private Transform target;
    private RectTransform rectTrans;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(target.position);
            rectTrans.position = pos;
        }
    }

    public void SetFollowTarget(Transform target)
    {
        this.target = target;
    }

    public void SetUsername(string username)
    {
        usernameText.text = username;
    }

    public void UpdateHpGui(float hp)
    {
        hbBarImg.fillAmount = hp / 100;
    }
}
