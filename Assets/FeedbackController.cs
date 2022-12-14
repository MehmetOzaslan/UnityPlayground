using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    public Sensor inputSensor;
    public Actuator outputActuator;

    [SerializeField]
    public float kp = 5;
    [SerializeField]
    public float ki = 5;
    [SerializeField]
    public float kd = 4;

    public float u = 0;
    float iErr = 0;
    float dErr = 0;
    public float e = 0;
    public float ePrev = 0;

    public bool isInitialized()
    {
        return inputSensor != null && outputActuator != null;
    }

    void FixedUpdate()
    {
        if (!isInitialized())
            return;

        //Timestep approximations
        float dt = Time.fixedDeltaTime;
        float ddt = dt / 10000;

        //Calculate Error
        e = inputSensor.getTarget() - inputSensor.getCurrent();
        iErr = iErr + e * dt;
        dErr = (e - ePrev)/dt;

        //Signal
        u = kp * e + kd*dErr + ki*iErr;

        //Output signal to actuator.
        outputActuator.useSignal(u);

        ePrev = e;
    }
}


[CustomEditor(typeof(FeedbackController))]
public class ControllerEditor : Editor
{
    Camera sceneCamera;
    private void Awake()
    {
        sceneCamera = SceneView.GetAllSceneCameras()[0];
    }
    // Custom in-scene UI for when ExampleScript
    // component is selected.
    public void OnSceneGUI()
    {
        var t = target as FeedbackController;
        var tr = t.transform;
        var pos = tr.position;
        var color = new Color(1, 0.8f, 0.4f, 1);
        Handles.color = color;
        Handles.DrawWireDisc(pos, sceneCamera.transform.forward, 1.0f);
        // display object "value" in scene
        GUI.color = color;

        string label = "Not Initialized";
        if (t.isInitialized())
        {
            label = "e:" + t.e + " t:" + t.inputSensor.getTarget() + " u:" + t.u;
        }
        Handles.Label(pos, label);
    }
}