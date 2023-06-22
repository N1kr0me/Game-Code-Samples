using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RamAttack : MonoBehaviourPunCallbacks
{
    private bool isColliding = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isColliding)
        {
            // Get the player health component of the other player
            PlayerHealth otherHealth = other.gameObject.GetComponent<PlayerHealth>();

            // Check if the other player is currently dashing
            TPController myController = GetComponent<TPController>();
            TPController otherController = other.gameObject.GetComponent<TPController>();
            if (otherController != null && myController.isDashing)
            {
                // Reduce other player's health by 100
                if (otherHealth != null)
                {
                    otherHealth.RamDamage(100);
                }
            }

            // Set isColliding to true to prevent multiple collisions in one frame
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Reset isColliding when the collision ends
            isColliding = false;
        }
    }
}