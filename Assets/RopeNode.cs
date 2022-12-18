using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FeedbackController), typeof(Rigidbody))]
public class RopeNode : MonoBehaviour, Actuator, Sensor
{
    public float maximumDistance = 2;
    public float distance = 1;
    public FeedbackController controller;
    public Rigidbody rb;
    public GameObject parent;
    public GameObject child;

    public void FixedUpdate()
    {
        //if((transform.position - parent.transform.position).magnitude > maximumDistance){
        //    transform.position = (transform.position - parent.transform.position).normalized * maximumDistance;
        //}
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<FeedbackController>();
        controller.SetAcuator(this);
        controller.SetSensor(this);
    }


    void Actuator.useSignal(float u)
    {
        if (parent == null || child == null) return;

        Vector3 pdir = transform.position - parent.transform.position;
        Vector3 cdir = transform.position - child.transform.position;

        Vector3 chosenDir = (cdir + pdir).normalized;

        rb.AddForce(chosenDir * u);
    }

    float Sensor.getCurrent()
    {
        if (parent == null) return distance;
        Vector3 pdir = transform.position - parent.transform.position;
        Vector3 cdir = transform.position - child.transform.position;
        return (cdir + pdir).magnitude;
    }

    float Sensor.getTarget()
    {
        return distance;
    }
}
