using System.Collections;
using UnityEngine;

public class Meteorite : MonoBehaviour {
    private MeteoriteData myData;
    private Vector3 going;
    public float speed;

    public bool bounced;
    public int AmountSmall;

    public float turnSpeed;
    public void Fill(MeteoriteData _data, Vector2 goingLoc, Vector2 _speed) {
        myData = _data;
        going = goingLoc;
        speed = Random.Range(_speed.x, _speed.y);
        turnSpeed = Random.Range(5.0f, 20.0f) / 100; ;
        Debug.Log(turnSpeed);
        StartCoroutine(Move());
    }

    public void SetDirection(Vector3 dir,float force) {

        g = dir * 1000000 + MeteoritesManager.instance.Player.transform.position;
        speed += 0.5f;
    }

    Vector3 g;
    private IEnumerator Move() {
        g = (going - transform.position) * 10;
        Vector3 start = transform.position;
        while (transform.position != g) {
            yield return null;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y ,(transform.eulerAngles.z + turnSpeed));
            if (Vector3.Distance(transform.position, start) > 5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
        }
        MeteoritesManager.instance.NewMeteorite();
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Meteorite>() != null) {
            if (bounced == false) {
                bounced = true;
                Meteorite other = collision.gameObject.GetComponent<Meteorite>();
                other.bounced = true;

                Vector3 dirA = g - transform.position;
                Vector3 dirB = other.g - other.transform.position;
                Debug.Log(Vector3.Distance(dirA, dirB));
                bool sameDir = false;
                if (Vector3.Distance(dirA, dirB) < 20) {
                    sameDir = true;
                    float diff = Random.Range(0.1f, 1.0f);
                    if (speed < other.speed) {

                        speed += diff;
                        other.speed -= diff;
                    } else {
                        speed -= diff;
                        other.speed += diff;
                    }
                } else {
                    speed -= Random.Range(0.1f, 1.0f);
                }

                Vector3 temp = Vector3.MoveTowards(g, other.g, Random.Range(0.0f, 10.0f));
                g = Vector3.MoveTowards(other.g, g, Random.Range(0.0f, 10.0f));
                other.g = temp;
                bool toMuchSpeed = false;
                if (!sameDir) {
                    if (speed + other.speed > 6) {
                        toMuchSpeed = true;
                    }
                } else if (Mathf.Abs(speed - other.speed) > 2) {
                    toMuchSpeed = true;
                }
                other.Bounce(toMuchSpeed);
                Bounce(toMuchSpeed);
            }
        } else if (collision.gameObject.GetComponent<MiniMeteorite>() != null) {
            bounced = true;
            Destroy(collision.gameObject);
        } else if (collision.gameObject.GetComponent<Movement>() != null) {
            if (myData.isPositiveM) {
                Health.Instance.Increment(myData.id - 1);
                MeteoritesManager.instance.NewMeteorite();
                Destroy(gameObject);
            } else {
                Health.Instance.Decrement();
                Explode();
            }
        }
    }

    public void Bounce(bool _speed) {
        if (Random.Range(0, 100) < 50 || _speed || speed < 2) {
            Explode();
        }
        StartCoroutine(B());
    }

    private void Explode() {
        Vector3 tempPos = transform.position;
        tempPos.z = -0.4f;
        Instantiate(MeteoritesManager.instance.explosion, tempPos, Quaternion.identity);
        if(myData.smallGObj) {
            for (int i = 0; i < AmountSmall; i++) {
                GameObject t = Instantiate(myData.smallGObj, transform.position, Quaternion.identity);
                t.transform.parent = null;
                Vector2 temp = new Vector2(Random.Range(g.x + 10, g.x - 10), Random.Range(g.y + 10, g.y - 10));
                t.GetComponent<MiniMeteorite>().Fill(temp, speed);
            }
        }

        MeteoritesManager.instance.NewMeteorite();
        Destroy(gameObject);
    }

    private IEnumerator B() {
        yield return null;
        yield return null;
        bounced = false;
    }
}
