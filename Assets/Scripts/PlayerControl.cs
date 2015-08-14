using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 700f;
    public bool facingR = true;

    Animator anim;

    bool grounded = false;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool doubleJump = false;

    private PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        if (!PhotonNetwork.offlineMode)
        {
            photonView = GetComponent<PhotonView>();

            if (!photonView.isMine)
            {
                this.enabled = false;
                GetComponent<PlayerShootingControl>().enabled = false;
                GetComponent<PlayerNetworkControl>().enabled = true;
            }
            else
            {
                GetComponent<PlayerShootingControl>().enabled = true;
                GetComponent<PlayerNetworkControl>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        anim.SetBool("Grounded", grounded);

        if (grounded)
            doubleJump = false;

        float move = Input.GetAxis("Horizontal");

        anim.SetFloat("MoveSpeed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !facingR)
        {
            if (PhotonNetwork.offlineMode)
                Flip();
            else
                photonView.RPC("Flip", PhotonTargets.All);
        }
        else if (move < 0 && facingR)
        {
            if (PhotonNetwork.offlineMode)
                Flip();
            else
                photonView.RPC("Flip", PhotonTargets.All);
        }

        anim.SetFloat("JumpSpeed", GetComponent<Rigidbody2D>().velocity.y);
    }

    void Update()
    {
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));

            if (!grounded && !doubleJump)
                doubleJump = true;
        }
    }

    [PunRPC]
    void Flip()
    {
        facingR = !facingR;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}