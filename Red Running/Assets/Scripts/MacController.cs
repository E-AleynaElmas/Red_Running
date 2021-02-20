using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MacController : MonoBehaviour
{
    GameObject[] pointsToVisit;
    bool distanceControl = true;
    Vector3 distance;
    int distanceCounter = 0;
    bool backOrFront = true;

    [SerializeField]
    GameObject playerCharacter;

    RaycastHit2D ray;

    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    Sprite frontSide;
    [SerializeField]
    Sprite backSide;

    int speed = 3;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject bullet;

    float fireTime = 0;
    void Start()
    {
        pointsToVisit = new GameObject[transform.childCount];
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < pointsToVisit.Length; i++)
        {
            pointsToVisit[i] = transform.GetChild(0).gameObject;
            pointsToVisit[i].transform.parent = transform.parent;
        }
    }

    void FixedUpdate()
    {
        Spying();

        if (ray.collider.tag == "Player")
        {
            speed = 3;
            spriteRenderer.sprite = frontSide;
            Fire();
            Debug.Log("gördü");
        }
        else
        {
            speed = 1;
            spriteRenderer.sprite = backSide;
            Debug.Log("Görmedi");
        }

        FollowThePoints();
    }

    void Spying()
    {
        Vector3 raydDirection = playerCharacter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, raydDirection, 1000, layerMask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }

    void Fire()
    {
        fireTime += Time.deltaTime;
        if(fireTime > Random.RandomRange(0.5f, 1.5f))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            fireTime = 0;
        }
    }

    void FollowThePoints()
    {
        if (distanceControl)
        {
            distance = (pointsToVisit[distanceCounter].transform.position - transform.position).normalized;
            distanceControl = false;
        }
        float distance1 = Vector3.Distance(transform.position, pointsToVisit[distanceCounter].transform.position);
        transform.position += distance * Time.deltaTime * speed;
        if (distance1 < 0.5f)
        {
            distanceControl = true;

            if (distanceCounter == pointsToVisit.Length - 1)
            {
                backOrFront = false;
            }
            else if (distanceCounter == 0)
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

    public Vector2 GetDirection()
    {
        return (playerCharacter.transform.position - transform.position).normalized;
    }

//Sadece scene ekranında görülür, oyunda görülmez
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 0.3f);
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif
}

//Editör kodu kalıbı
[SerializeField]
#if UNITY_EDITOR
[CustomEditor(typeof(MacController))]
[System.Serializable]

class MacControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MacController script = (MacController)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("GENERATE", GUILayout.MinWidth(1), GUILayout.MinHeight(1)))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();
        }

        EditorGUILayout.Space();
        //public veya SerializeField objeleri editörde görebilmek için yazılır.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerCharacter"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("frontSide"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backSide"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bullet"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif
