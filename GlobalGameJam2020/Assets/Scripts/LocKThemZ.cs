using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocKThemZ : MonoBehaviour
{
    float z;
    void Start()
    {
        z = transform.position.z;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
