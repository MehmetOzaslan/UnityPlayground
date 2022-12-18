using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    public Sensor inputSensor;
    public Actuator outputActuator;

    [SerializeField]
    public float kp = 3;
    [SerializeField]
    public float ki = 0;
    [SerializeField]
    public float kd = 3;

    public float u = 0;
    float iErr = 0;
    float dErr = 0;
    public float e = 0;
    public float ePrev = 0;

    public OvershootCalculator overshoot;

    public void SetAcuator(Actuator actuator) { this.outputActuator = actuator; }
    public void SetSensor(Sensor sensor) { this.inputSensor = sensor; }

    public bool isInitialized()
    {
        return inputSensor != null && outputActuator != null;
    }

    private void Awake()
    {
        overshoot = new OvershootCalculator();
    }

    void FixedUpdate()
    {
        if (!isInitialized())
            return;

        //Determine any overshoot.
        overshoot.Tick(inputSensor);

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

public class OvershootCalculator{

    public float overshoot = 0;
    public float oldTarget = 0;
    public int direction = 0;


    public void Tick(Sensor sensor) {

        float error = sensor.getCurrent() - sensor.getTarget();

        //Setting the direction when the target changes and resetting the overshoot
        if (sensor.getTarget() != oldTarget) {
            oldTarget = sensor.getTarget();
            direction = (int)Mathf.Sign(error);
            overshoot = 0;
        }

        //Once the object crosses over the target.
        if ((int)Mathf.Sign(error) == -direction ) {
            overshoot = Mathf.Max( Mathf.Abs(overshoot), Mathf.Abs(error));
        }
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
            label = "e:" + t.e + "\nt:" + t.inputSensor.getTarget() + "\nu:" + t.u + "\nMp:" + t.overshoot.overshoot;
        }
        Handles.Label(pos, label);
    }
}