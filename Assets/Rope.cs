using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//The idea: Can you use PID to model a rope?
public class Rope : MonoBehaviour
{

    public float length = 10;
    public List<RopeNode> nodes;
    public int nodeCount;

    public GameObject ropeNode;

    // Start is called before the first frame update
    void Start()
    {

        float distance = length / nodeCount;

        for (int i = 0; i < nodeCount; i++)
        {
            RopeNode child = Instantiate(ropeNode, gameObject.transform).GetComponent<RopeNode>();
            nodes.Add(child);

        }

        for (int i = 0; i < nodeCount; i++)
        {
     
            nodes[i].GetComponent<Rigidbody>().transform.position = new Vector3(i, 0, 0);

            if (i > 0 && i < nodeCount-1)
            {
                nodes[i].parent = nodes[i - 1].gameObject;
                nodes[i].child = nodes[i + 1].gameObject;
                nodes[i].distance = distance;
            }


            if (i == 0 || i == nodeCount-1)
            {
                nodes[i].GetComponent<Rigidbody>().isKinematic = true;
            }

        }


    }
}

[CustomEditor(typeof(Rope))]

public class RopeEditor : Editor
{

    public float kp = 1;
    public float ki = 1;
    public float kd = 1;
    public float damping = 1;
    public float mass = 1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Rope t = target as Rope;

        GUILayout.BeginVertical("GroupBox");
        GUILayout.Label("Control Parameters");
        kp  =  EditorGUILayout.Slider("kp", kp, 0, 20);
        ki = EditorGUILayout.Slider("ki",ki, 0, 20);
        kd = EditorGUILayout.Slider("kd", kd, 0, 20);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox");
        GUILayout.Label("Physics Parameters");
        damping = EditorGUILayout.Slider("damping", damping, 0, 20);
        mass = EditorGUILayout.Slider("mass", mass, 0, 20);
        GUILayout.EndVertical();

        foreach (RopeNode node in t.nodes)
        {
            node.rb.drag = damping;
            node.rb.mass = mass;
            node.controller.kp = kp;
            node.controller.ki = ki;
            node.controller.kd = kd;
        }

    }
}
