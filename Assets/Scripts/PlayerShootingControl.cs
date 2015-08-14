using UnityEngine;
using System.Collections;

public class PlayerShootingControl : Photon.MonoBehaviour
{
    public Transform shootPoint;
    Animator anim;

    public Rigidbody2D bullet;
    public float bulletSpeed;
    PlayerControl playerScript;

    public int ammo = 60, clipSize = 15, shotsFired, leftAmmo, counter;
    bool reload;

    public GUIText ammoText;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = GetComponent<PlayerControl>();
        leftAmmo = ammo;
        ammoText = GameObject.FindWithTag("AmmoText").GetComponent<GUIText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !reload && leftAmmo > 0)
        {
            anim.SetTrigger("Shoot");
            shotsFired++;

            if (playerScript.facingR)
            {
                // ... instantiate the rocket facing right and set it's velocity to the right. 

                if (PhotonNetwork.offlineMode)
                {
                    Rigidbody2D bullets = Instantiate(bullet, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                    bullets.velocity = new Vector2(bulletSpeed, 0);
                }
                else
                {
                    GameObject bullets = PhotonNetwork.Instantiate("Prefab/bullet", shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)), 0) as GameObject;
                    bullets.GetComponent<PhotonView>().RPC("Shoot", PhotonTargets.All, new Vector2(bulletSpeed, 0));
                }
            }

            else
            {
                if (PhotonNetwork.offlineMode)
                {
                    Rigidbody2D bullets = Instantiate(bullet, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 180))) as Rigidbody2D;
                    bullets.velocity = new Vector2(-bulletSpeed, 0);
                }
                else
                {
                    GameObject bullets = PhotonNetwork.Instantiate("Prefab/bullet", shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)), 0) as GameObject;
                    bullets.GetComponent<PhotonView>().RPC("Shoot", PhotonTargets.All, new Vector2(-bulletSpeed, 0));
                }
            }

            leftAmmo = ammo - shotsFired;
            if ((leftAmmo % clipSize == 0))
            {
                if (counter == 0)
                {
                    StartCoroutine("Reload");
                    counter++;
                }
            }
        }

        ammoText.text = leftAmmo + "/" + ammo;
    }

    IEnumerator Reload()
    {

        anim.SetTrigger("Reload");
        reload = true;
        yield return new WaitForSeconds(1);
        reload = false;
        counter = 0;
        StopCoroutine("Reload");
    }

}
