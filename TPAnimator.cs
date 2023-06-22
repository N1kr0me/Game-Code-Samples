using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb= this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed",rb.velocity.magnitude/maxSpeed);
    }
}
