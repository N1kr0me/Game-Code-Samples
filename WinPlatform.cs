using UnityEngine;
using Photon.Pun;

public class WinPlatform : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the colliding player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            // Check if the player's score is equal to or above 500
            if (playerHealth != null && playerHealth.Score >= 500)
            {
                // Get the FruitType of the PlayerHealth script
                string WinnerFruit = playerHealth.FruitType;
                
                 // Broadcast the win event to all players using an RPC
                photonView.RPC("WinGame", RpcTarget.All, WinnerFruit);
            }
        }
    }

    [PunRPC]
    private void WinGame(string WinnerFruit)
    {
        // Transition to the WinScene
        PhotonNetwork.LoadLevel("WinScene");
    }
}