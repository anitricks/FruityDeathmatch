using UnityEngine;
using System.Collections;

public class Bullet : Photon.PunBehaviour
{
    public GameObject explosion;

    Rigidbody2D body2d;


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
        if (photonView.isMine)
        {
            // TODO: dmg only to differnet team members

            string tag = other.tag;

            if (tag == "Player")
            {
                Player player = other.GetComponent<Player>();

                // TODO: dmg based on weapon
                player.photonView.RPC("ApplyDmg", PhotonTargets.All, 10);
            }

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
