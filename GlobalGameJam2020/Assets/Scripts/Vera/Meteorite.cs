using System.Collections;
using UnityEngine;

public class Meteorite : MonoBehaviour {
    private MeteoriteData myData;
    private Vector3 going;
    public float speed;

    public bool bounced;
    public int AmountSmall;
    public void Fill(MeteoriteData _data, Vector2 goingLoc, Vector2 _speed) {
        myData = _data;
        going = goingLoc;
        speed = Random.Range(_speed.x, _speed.y);
        StartCoroutine(Move());
    }

    public void SetDirection(Vector3 dir,float force) {
        g = Vector3.MoveTowards(g,dir,force * Time.deltaTime);
    }

    Vector3 g;
    private IEnumerator Move() {
        g = (going - transform.position) * 10;
        Vector3 start = transform.position;
        while (transform.position != g) {
            yield return null;
            if (Vector3.Distance(transform.position, start) > 5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
        }
        MeteoritesManager.instance.NewMeteorite();
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision) {
        Debug.LogError("Collision");
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
            Debug.LogError("DESTROYED!");
            bounced = true;
            Destroy(collision.gameObject);
        } else if (collision.gameObject.GetComponent<Movement>() != null) {
            if (myData.isPositiveM) {
                print("Good Object!");
            } else {
                print("Bad Object!");
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
        if(myData.parts.Count != 0) {
            for (int i = 0; i < myData.parts.Count; i++) {
                GameObject t = Instantiate(myData.parts[i], transform.position, Quaternion.identity);
                t.transform.parent = null;
                Vector2 temp = new Vector2(Random.Range(g.x + 10, g.x - 10), Random.Range(g.y + 10, g.y - 10));
                t.GetComponent<MiniMeteorite>().Fill(temp, speed);
            }
        } else {
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
