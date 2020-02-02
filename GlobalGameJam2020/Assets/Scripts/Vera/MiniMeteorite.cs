using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMeteorite : MonoBehaviour
{
    private Vector3 going;
    private float speed;
    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Fill(Vector2 goingLoc, float _speed) {
        going = goingLoc;
        speed = _speed + Random.Range(1.1f , 3.0f);
        StartCoroutine(Move());
        StartCoroutine(Delay());
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

    private void OnCollisionEnter(Collision collision) {
        Movement movement = collision.transform.GetComponent<Movement>();
        if (movement != null) {
            movement.SetVelocity(rigidbody.velocity * 2);
            Destroy(gameObject);
        }
    }
}
