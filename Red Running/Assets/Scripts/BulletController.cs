using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    MacController mac;
    Rigidbody2D physics;

    void Start()
    {
        mac = GameObject.FindGameObjectWithTag("TagMac").GetComponent<MacController>();
        physics = GetComponent<Rigidbody2D>();
        physics.AddForce(mac.GetDirection()*500);
    }

}
