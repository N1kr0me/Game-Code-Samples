using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class TPController : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashForce = 10f;
    public float dashDuration = 0.5f;
    public float shootForce = 100f;
    public float shootCooldown = 1f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Animator animator;
    public Rigidbody rb;
    private float horizontalMove;
    private float verticalMove;
    private bool isJumping;
    public bool isDashing;
    public bool isGrounded;
    public bool canRAM = false;
    private float dashTimer;
    private float shootTimer;
    private new PhotonView photonView;
    private PlayerControls playerInput;
    public Camera Camera;
    [SerializeField] private float distanceToGround = 0.1f;

    //Audio clips
    //private AudioSource audioSource;

    //public AudioClip JumpSound;
    //public AudioClip RamSound;
    //public AudioClip ShootSound;


    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInput = new PlayerControls();
        if(photonView.IsMine)
        {
            playerInput.Enable();
        }
    }

    public override void OnLeftRoom()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
            //audioSource.enabled = false;
        }
    }

    private void Update()
    {
        if(!photonView)
        {
            Debug.LogError("Error at PhotonView");
            return;
        }
        if (photonView.IsMine)
        {

            horizontalMove = playerInput.Player.Movement.ReadValue<Vector2>().x;
            verticalMove = playerInput.Player.Movement.ReadValue<Vector2>().y;
            isJumping = playerInput.Player.Jump.ReadValue<float>() > 0.5f;
            isDashing = playerInput.Player.Ram.ReadValue<float>() > 0.1f && isGrounded;
            if (playerInput.Player.Shoot.ReadValue<float>() > 0.5f && Time.time > shootTimer)
            {
                photonView.RPC("Shoot", RpcTarget.All);
                shootTimer = Time.time + shootCooldown;
            }

            // Update Animator parameters
            if(isJumping)
            {
                animator.SetTrigger("Jump");
                //audioSource.PlayOneShot(JumpSound);
            }
            if(isDashing && canRAM)
            {
                animator.SetTrigger("Ram");
                //audioSource.PlayOneShot(RamSound);
            }
            
            animator.SetFloat("Speed", rb.velocity.magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            // Check if the player is grounded
            
            isGrounded=Grounded();

            // Move the player
            Vector3 moveDirection = new Vector3(horizontalMove, 0f, verticalMove).normalized;
            moveDirection = Camera.transform.TransformDirection(moveDirection);
            moveDirection.y= 0f;

            rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            if(moveDirection!= Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
            // Jump
            if (isJumping && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Dash
            if (isDashing && canRAM)
            {
                dashTimer = dashDuration;
                rb.AddForce(moveDirection * dashForce, ForceMode.VelocityChange);
                canRAM= false;
            }

            // Decrement dash timer
            if (dashTimer > 0)
            {
                dashTimer -= Time.fixedDeltaTime;
                if (dashTimer <= 0)
                {
                    isDashing = false;
                }
            }
        }
    }

    [PunRPC]
    void Shoot()
    {
        // Spawn bullet and apply force in the forward direction
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);

        // Snap to front and play shoot animation
        transform.LookAt(transform.position + transform.forward);
        animator.SetTrigger("Shoot");
        //audioSource.PlayOneShot(ShootSound);
    }
}