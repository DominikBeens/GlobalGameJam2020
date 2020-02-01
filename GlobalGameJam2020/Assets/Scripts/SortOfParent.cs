using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOfParent : MonoBehaviour
{
    public Transform child;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = transform.position;        
        child.transform.position = temp;
    }
}
