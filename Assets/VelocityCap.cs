using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCap : MonoBehaviour
{
    public float maxVelocity = 100;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); ;
    }
}
