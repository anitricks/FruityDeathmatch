using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    // TODO: Spawn Points
    public Transform Team_1_Spawn_Point;
    public Transform Team_2_Spawn_Point;

    public Transform Power_Up_Spawn_Point;

    public static Level instance { get; private set; }

    void Awake()
    {
        instance = this;
        tag = "Level";
    }
}
