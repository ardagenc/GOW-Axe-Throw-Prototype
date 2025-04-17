using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    public float playerSpeedMax;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    public bool isGrounded = true;

    [Header("Camera")]
    public ThirdPersonCam cameraScript;
    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public bool isAiming;

    [Header("")]
    public Axe axeScript;
    public WeaponScript weaponScript;
    public bool hasAxe = true;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;

    Rigidbody rb;
    public Animator anim;


    public float groundRaycastLength;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void FixedUpdate() 
    {

        isGrounded = Physics.Raycast(transform.position + new Vector3(0,1,0), Vector3.down, playerHeight, Ground);

        MovePlayer();

        SpeedControl();


        //handle drag

        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        
    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Animation
        if((Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0) && isGrounded)
        {
            anim.SetBool("isWalking", true);
        }
        else if(!(Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0) && isGrounded)
        {
            anim.SetBool("isWalking", false);
        }

        //Aim

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Aim();

            if(isAiming)
            {
                anim.SetBool("isAiming", true);
            }
            else
            {
                anim.SetBool("isAiming", false);
            }
        }

        if(Input.GetKeyDown(KeyCode.E) && isAiming)
        {
            hasAxe = false;
            anim.SetTrigger("isThrowing");
            anim.SetBool("hasAxe", false);
            //axeScript.AxeThrow();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            axeScript.AxeReturn();
            anim.SetBool("isPulling", true);

        }
        
    }

    public void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(isGrounded)
        {
            rb.AddForce(movementDirection.normalized * playerSpeed, ForceMode.Force);
        }
        else if(!isGrounded)
        {
                rb.AddForce(movementDirection.normalized * playerSpeed * airMultiplier, ForceMode.Force);
        }

    }

    public void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > playerSpeedMax)
        {
            Vector3 limitedVel = flatVel.normalized * playerSpeedMax;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0,1,0), transform.position + Vector3.down * groundRaycastLength);
    }

    public void Aim()
    {
        if(cameraScript.currentStyle == ThirdPersonCam.CameraStyle.Basic)
        {
            isAiming = true;
            thirdPersonCam.gameObject.SetActive(false);
            cameraScript.currentStyle = ThirdPersonCam.CameraStyle.Combat;

        }
        else if(cameraScript.currentStyle == ThirdPersonCam.CameraStyle.Combat)
        {
            isAiming = false;
            thirdPersonCam.gameObject.SetActive(true);
            cameraScript.currentStyle = ThirdPersonCam.CameraStyle.Basic;
        }
    }
}
