using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMeteorite : MonoBehaviour
{
    private Vector3 going;
    private float speed;
    public void Fill(Vector2 goingLoc, float _speed) {
        going = goingLoc;
        Debug.Log(going);
        speed = _speed + Random.Range(1.1f , 3.0f)/100;
        Debug.LogError(speed);
        StartCoroutine(Move());
        StartCoroutine(Delay());
    }

    private IEnumerator Move() {
        Vector3 g = going;
        Vector3 start = transform.position;
        while (transform.position != g) {
            yield return null;
            if (Vector3.Distance(transform.position, start) > 5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
        }
        Destroy(gameObject);
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
    }
}
