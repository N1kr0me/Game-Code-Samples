using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JuiceBlob : MonoBehaviourPunCallbacks
{
    public string FruitType;
    public int index;
    public int juiceAmount = 1;
    public float destroyTime = 15f;

    public float damping = 1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        rb.velocity *= 1f - damping * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.photonView.IsMine && playerHealth.FruitType != FruitType)
            {
                playerHealth.AddToInventory(index, juiceAmount);
                photonView.RPC("DestroyBlob", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void DestroyBlob()
    {
        if (photonView != null)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}