using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Saw : MonoBehaviour
{
    GameObject []pointsToVisit;
    bool distanceControl = true;
    Vector3 distance;
    int distanceCounter = 0;
    bool backOrFront = true;

    void Start()
    {
        pointsToVisit = new GameObject[transform.childCount];

        for(int i = 0; i < pointsToVisit.Length; i++)
        {
            pointsToVisit[i] = transform.GetChild(0).gameObject;
            pointsToVisit[i].transform.parent = transform.parent;
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
        FollowThePoints();
    }

    void FollowThePoints()
    {
        if (distanceControl)
        {
            distance = (pointsToVisit[distanceCounter].transform.position - transform.position).normalized;
            distanceControl = false;
        }
        float distance1 = Vector3.Distance(transform.position, pointsToVisit[distanceCounter].transform.position);
        transform.position += distance * Time.deltaTime * 10;
        if (distance1 < 0.5f)
        {
            distanceControl = true;

            if (distanceCounter == pointsToVisit.Length - 1)
            {
                backOrFront = false;
            }
            else if(distanceCounter == 0)
            {
                backOrFront = true;
            }

            if (backOrFront)
            {
                distanceCounter++;
            }
            else
            {
                distanceCounter--;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Saw))]
[System.Serializable]

class SawEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Saw script = (Saw)target;
        if (GUILayout.Button("GENERATE",GUILayout.MinWidth(100), GUILayout.MinHeight(100)))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();
        }
    }
}
#endif