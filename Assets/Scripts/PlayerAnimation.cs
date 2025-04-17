using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.isGrounded && rb.velocity.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else if(playerMovement.isGrounded && rb.velocity.magnitude < 2)
        {
            anim.SetBool("isWalking", false);
        }
    }
}
