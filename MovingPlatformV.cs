using UnityEngine;
using Photon.Pun;

public class MovingPlatformV : MonoBehaviourPun
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 1f;


    private new PhotonView photonView;

    
    private Vector3 currentTarget;

    private void Start()
    {
        currentTarget = startPoint.position;
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (transform.position == currentTarget)
        {
            if (currentTarget == startPoint.position)
                currentTarget = endPoint.position;
            else
                currentTarget = startPoint.position;

            // Synchronize platform position across the network
            photonView.RPC("SyncPlatformPosition", RpcTarget.Others, transform.position, currentTarget);
        }
    }

    [PunRPC]
    void SyncPlatformPosition(Vector3 position, Vector3 target)
    {
        transform.position = position;
        currentTarget = target;
    }
}