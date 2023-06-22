using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AccCheese : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                TPController tpController = other.gameObject.GetComponent<TPController>();
                if (tpController != null)
                {
                    tpController.canRAM = true;
                    
                   // photonView.RPC("DestroyProp", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void DestroyProp()
    {
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}