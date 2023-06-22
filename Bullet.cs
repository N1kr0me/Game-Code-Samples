using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed = 10f;
    public float destroyTime = 5f;
    public int damage = 100;
    public string Fruit ;

    private new PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        // Destroy the bullet after destroyTime seconds if it doesn't collide with anything
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with is a player
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && player.FruitType!=Fruit)
        {
            // Reduce the player's health by the bullet's damage
            player.TakeDamage(damage);

            // Destroy the bullet
            photonView.RPC("DestroyBullet", RpcTarget.All);
        }
    }

    [PunRPC]
    void DestroyBullet()
    {
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}