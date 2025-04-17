using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Axe : MonoBehaviour
{
    public WeaponScript weaponScript;

    public Rigidbody rb;
    public Animator anim;

    public float throwPower;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    public Transform hand;

    public Transform axeTransform;
    public Transform target, curvePoint;
    private Vector3 oldPosition;
    private float time = 0.0f;

    public bool isReturning = false;

    private void Start() 
    {
        origLocPos = axeTransform.localPosition;
        origLocRot = axeTransform.localEulerAngles;
    }

    private void Update() 
    {
        if(isReturning)
        {
            if(time < 1.0f)
            {
                axeTransform.position = getBezierQuadraticCurvePoint(time, oldPosition, curvePoint.position, target.position);
                time += Time.deltaTime * 1.5f;
            }
            else
            {
                CatchAxe();
            }
        }    
    }

    public void AxeThrow()
    {
        rb.transform.parent = null;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        axeTransform.eulerAngles = new Vector3(0, 90 + transform.eulerAngles.y, 0);
        rb.AddForce(Camera.main.transform.forward * throwPower + transform.up * 2, ForceMode.Impulse);
        weaponScript.activated = true;
        anim.SetBool("isThrowing", false);

        Debug.Log(axeTransform.transform.position);
    }

    public void AxeReturn()
    {
        oldPosition = axeTransform.position;
        isReturning = true;
        rb.isKinematic = true;
        // axeTransform.DORotate(new Vector3(-90, 90, 180), .1f).SetEase(Ease.InOutSine);
        //axeTransform.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        weaponScript.activated = true;
    }

    public void CatchAxe()
    {
        time = 0;
        isReturning = false;
        axeTransform.parent = hand;
        weaponScript.activated = false;
        axeTransform.localEulerAngles = origLocRot;
        axeTransform.localPosition = origLocPos;

        GetComponent<PlayerMovement>().anim.SetBool("isPulling", false);
        GetComponent<PlayerMovement>().hasAxe = true;

        anim.SetBool("hasAxe", true);


    }

    Vector3 getBezierQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1-t;
        float tt = t*t;
        float uu = u*u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}
