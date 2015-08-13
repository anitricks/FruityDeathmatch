using UnityEngine;
using System.Collections;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance { get; private set; }

    public string username;// { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public void SetUsername(string username)
    {
        this.username = username;
    }

}
