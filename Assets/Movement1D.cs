using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Movement1D : MonoBehaviour
{
    [SerializeField]
    float x=0;
    [SerializeField]
    float v =1;
    [SerializeField]
    float a =1;

    //How far predictions are made.
    float timeRange = 3f;

    //Timesteps for predictions are located
    int calculationCount = 10;

    float calculationPosition(float t) {
        return x + v * t + Mathf.Sign(a)* Mathf.Pow(a * t, 2)/2;
    }

    public float[] getPositions() {
        float[] positions = new float[10];
        for (int i = 0; i < calculationCount; i++)
        {
            float t = Mathf.Lerp(0, 3, (float)i / (float)calculationCount);
            positions[i] = calculationPosition(t);
        }
        return positions;
    }
}

[CustomEditor(typeof(Movement1D))]
public class ExampleEditor : Editor
{
    // Custom in-scene UI for when ExampleScript
    // component is selected.
    public void OnSceneGUI()
    {
        var t = target as Movement1D;
        foreach (var pos in t.getPositions())
        {
            Handles.DrawSolidRectangleWithOutline(new Rect(pos, 0, 0.3f, 0.3f), Color.green, Color.black);
        }
    }
}