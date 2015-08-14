using UnityEngine;
using System.Collections;

public class Bullet : Photon.PunBehaviour
{
    public GameObject explosion;

    Rigidbody2D body2d;


    // sync relavent
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (photonView.isMine)
        {
            Invoke("DestroyBullet", 2);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Dummy" && photonView.isMine)
        {
            Debug.Log("Hit");
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        PhotonNetwork.Destroy(photonView);
    }

    [PunRPC]
    void Shoot(Vector2 vel)
    {
        body2d.velocity = vel;
    }
}
