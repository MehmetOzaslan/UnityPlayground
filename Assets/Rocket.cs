using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, Sensor, Actuator
{

    public float maxForce = 10;
    [SerializeField]
    float targetX = 5;
    FeedbackController controller;
    Rigidbody rb;

    public float getCurrent()
    {
        return rb.position.x;
    }

    public float getTarget()
    {
        return targetX;
    }

    public void useSignal(float u)
    {
        if (u > maxForce) {
            u = maxForce;
        }

        rb.AddForce(new Vector3(u, 0, 0));
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) { rb = gameObject.AddComponent<Rigidbody>(); }
        rb.useGravity = false;

        controller = GetComponent<FeedbackController>();


        //Set the controller's actuators
        controller.inputSensor = this;
        controller.outputActuator = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
