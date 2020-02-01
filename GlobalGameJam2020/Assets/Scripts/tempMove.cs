using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMove : MonoBehaviour
{
    List<Vector3> temp = new List<Vector3>();
    int cur;
    private void Awake() {
        temp.Add(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z));
        temp.Add(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z));
        temp.Add(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z));
        temp.Add(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, temp[cur], 5 * Time.deltaTime);
        if(transform.position == temp[cur]) {
            cur++;
            if(cur == temp.Count) {
                cur = 0;
            }
        }
    }
}
