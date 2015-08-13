using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginInfoGuiController : MonoBehaviour
{
    public Text usernameText;

    public void OnLogin()
    {
        UserInfo.instance.SetUsername(usernameText.text);

        Application.LoadLevel("1_RegionSelection");
    }
}
