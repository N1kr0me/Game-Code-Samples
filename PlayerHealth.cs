using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float blobThrowForce = 5f;
    [SerializeField] private GameObject juiceBlobPrefab;
    [SerializeField] private GameObject DeathVFX;
    [SerializeField] private Transform blobSpawnPoint;
    [SerializeField] private Animator animator;
    
    private GameObject currentVFX;

    private int currentHealth;
    public int[] inventory = new int[6];
    public bool isDead;

    public string FruitType;
    public int Score=0;
    private new PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        for (int i = 1; i < 6; i++)
        {
            inventory[i] = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        if (photonView.IsMine)
        {
            if (isDead) return;

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animator.SetBool("Dead", true);
                photonView.RPC("Death", RpcTarget.All);
                //DropInventory();
                Invoke(nameof(Respawn), 10f);
            }
        }
    }

    public void RamDamage(int amount)
    {
        if (photonView.IsMine)
        {
            if (isDead) return;

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animator.SetBool("Dead", true);
                photonView.RPC("Death", RpcTarget.All);
                DropInventory();
                Invoke(nameof(Respawn), 10f);
            }
        }
    }

    private void Respawn()
    {
        isDead = false;
        animator.SetBool("Dead", false);
        currentHealth = maxHealth;
    }

    public void AddToInventory(int index, int amount)
    {
        inventory[index] += amount;
        Score += 50;
    }


    private void DropInventory()
    {
        for (int i = 0; i < 6; i++)
        {
            if (inventory[i] > 0)
            {
                for (int j = 0; j < inventory[i]; j++)
                {
                    //Puddle Spawn (future reference)
                    inventory[i] = 0;
                }
            }
        }
    }

    [PunRPC]
    void Death()
    {
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        GameObject blob = PhotonNetwork.Instantiate(juiceBlobPrefab.name, blobSpawnPoint.position + offset, Quaternion.identity);
        blob.GetComponent<Rigidbody>().AddForce(transform.forward * blobThrowForce, ForceMode.Impulse);
    }
    
    public void SpawnVFX()
    {
        if (currentVFX == null)
        {
            currentVFX = PhotonNetwork.Instantiate(DeathVFX.name, transform.position, transform.rotation);
            currentVFX.transform.SetParent(transform); // Set the spawn point as the parent for proper positioning
        }
    }

    public void DestroyVFX()
    {
        if (currentVFX != null)
        {
            PhotonNetwork.Destroy(currentVFX);
            currentVFX = null;
        }
    }
}