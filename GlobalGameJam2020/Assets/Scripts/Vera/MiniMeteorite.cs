using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMeteorite : MonoBehaviour
{
    private Vector3 going;
    private float speed;
    private new Rigidbody rigidbody;

    public void Fill(Vector2 goingLoc, float _speed) {
        rigidbody = GetComponent<Rigidbody>();
        going = goingLoc;
        speed = _speed + Random.Range(1.1f , 3.0f);
        StartCoroutine(Move());
        StartCoroutine(Delay());
        transform.localScale = Vector3.one * Random.Range(0.85f, 1.15f);
    }

    public void SetDirection(Vector3 dir, float force) {

        g = dir * 1000000 + MeteoritesManager.instance.Player.transform.position;
        speed += 0.5f;
    }

    Vector3 g;
    private IEnumerator Move() {
        g = going;
        Vector3 start = transform.position;
        while (transform.position != g) {
            yield return null;
            if (Vector3.Distance(transform.position, start) > 5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            if (MeteoritesManager.instance.GetSlow()) {
                transform.position = Vector3.MoveTowards(transform.position, g, (speed * Time.deltaTime) / 3);
            } else {
                transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
            }
        }
        Destroy(gameObject);
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
    }

    public void Explode() {
        Vector3 tempPos = transform.position;
        tempPos.z = -0.4f;
        Instantiate(MeteoritesManager.instance.explosion, tempPos, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Movement movement = collision.transform.GetComponent<Movement>();
        if (movement != null) {
            Destroy(gameObject);
        }
    }
}
