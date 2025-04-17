using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerObj;
    public Transform orientation;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;
    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start() {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void FixedUpdate() {
        
        //rotate orientation

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player obj
        
        if(currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if(inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);  
        }
        
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 combatDirToLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y,transform.position.z);
            orientation.forward = combatDirToLookAt.normalized;

            playerObj.forward = combatDirToLookAt.normalized;
        }
    }
}
