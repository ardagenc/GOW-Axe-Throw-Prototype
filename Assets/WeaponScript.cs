using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float rotationSpeed;
    public bool activated;
    public Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        if(activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        Debug.Log(transform.position);
        rb.isKinematic = true;
        activated = false;    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Breakable"))
        {
            other.GetComponent<Destructible>().Break();
        }    
    }
}
