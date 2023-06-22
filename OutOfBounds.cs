using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OutOfBounds : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a player
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine)
        {
            // Teleport the player to the spawn point
            other.transform.position = spawnPoint.position;
        }
    }
}