using UnityEngine;
using System.Collections;

public class PlayerNetworkControl : Photon.MonoBehaviour
{
    private Rigidbody2D body2d;

    private bool facingR = true;

    // sync relavent
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    void OnEnable()
    {
        body2d = GetComponent<Rigidbody2D>();
        body2d.isKinematic = true;
    }

    void OnDisable()
    {
        body2d.isKinematic = false;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(body2d.position);
            stream.SendNext(body2d.velocity);
        }
        else
        {
            Vector2 syncPosition = (Vector2)stream.ReceiveNext();
            Vector2 syncVelocity = (Vector2)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = body2d.position;
        }
    }

    void FixedUpdate()
    {
        SyncMovement();
    }

    void SyncMovement()
    {
        syncTime += Time.deltaTime;
        body2d.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }


}
