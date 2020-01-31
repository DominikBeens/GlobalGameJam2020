using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Meteorite : MonoBehaviour {
    private MeteoriteData myData;
    private Vector3 going;
    public float speed;

    public bool bounced;
    public int AmountSmall;
    public void Fill(MeteoriteData _data,Vector2 goingLoc,Vector2 _speed) {
        myData = _data;
        going = goingLoc;
        speed = Random.Range(_speed.x, _speed.y);
        StartCoroutine(Move());
    }

    Vector3 g;
    private IEnumerator Move() {
        g = (going - transform.position) * 10;
        Vector3 start = transform.position;
        while(transform.position != g) {
            yield return null;
            if(Vector3.Distance(transform.position,start)>5 && !MeteoritesManager.instance.Inbounds(transform.position)) {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, g, speed * Time.deltaTime);
        }
        MeteoritesManager.instance.NewMeteorite();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.LogError("Collision");
        if(collision.gameObject.GetComponent<Meteorite>() != null) {
            if (bounced == false) {
                bounced = true;
                Meteorite other = collision.gameObject.GetComponent<Meteorite>();
                other.bounced = true;

                Vector3 dirA = g - transform.position;
                Vector3 dirB = other.g - other.transform.position;
                Debug.Log(Vector3.Distance(dirA, dirB));
                if(Vector3.Distance(dirA,dirB) < 20) {
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

                Vector3 temp = Vector3.MoveTowards(g,other.g,Random.Range(0.0f,10.0f));
                g = Vector3.MoveTowards(other.g, g, Random.Range(0.0f, 10.0f));
                other.g = temp;
                other.Bounce();
                Bounce();
            }
        } else if (collision.gameObject.GetComponent<MiniMeteorite>() != null) {
            Debug.LogError("DESTROYED!");
            bounced = true;
            Destroy(collision.gameObject);
        }

    }

    public void Bounce() {
        if(Random.Range(0,100) < 50) {
            Explode();
        } else {

        }
        StartCoroutine(B());
    }

    private void Explode() {

        for (int i = 0; i < AmountSmall; i++) {
            GameObject t = Instantiate(myData.smallGObj, transform.position, Quaternion.identity);
            t.transform.parent = null;
            Vector2 temp = new Vector2(Random.Range(g.x + 10, g.x - 10), Random.Range(g.y + 10, g.y - 10));
            t.GetComponent<MiniMeteorite>().Fill(temp, speed);
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
