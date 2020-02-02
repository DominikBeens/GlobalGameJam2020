using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private Vector3 going;
    public float speed;
    public float turnSpeed;
    public bool bounced;
    public void Fill(Vector2 goingLoc, Vector2 _speed) {
        going = goingLoc;
        speed = Random.Range(_speed.x, _speed.y);
        turnSpeed = Random.Range(5.0f, 20.0f) / 100;
        StartCoroutine(Move());
    }
    public void SetDirection(Vector3 dir, float force) {

        g = dir * 1000000 + MeteoritesManager.instance.Player.transform.position;
        speed += 0.5f;
    }

    Vector3 g;
    private IEnumerator Move() {
        g = (going - transform.position) * 10;
        Vector3 start = transform.position;
        while (transform.position != g) {
            yield return null;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, (transform.eulerAngles.z + turnSpeed));
            if (Vector3.Distance(transform.position, start) > 5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
        }
        MeteoritesManager.instance.NewMeteorite();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Movement>() != null) {
            Use();
            MeteoritesManager.instance.NewMeteorite();
            Destroy(gameObject);
        } else if (collision.gameObject.GetComponent<MiniMeteorite>() != null && collision.gameObject.GetComponent<PowerUp>() != null) {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    public virtual void Use() {

    }
}
