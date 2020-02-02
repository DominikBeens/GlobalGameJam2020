using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MeteoritesManager : MonoBehaviour
{
    public static MeteoritesManager instance;
    public Transform Player;
    public float range;
    public float sizeMeteorite;
    public int randomChance;
    public float minSpeed;
    public float maxSpeed;
    public int maxAmount;

    public Vector4 bounds;

    public Health h;
    public List<MeteoriteData> badMeteorites = new List<MeteoriteData>();
    public List<MeteoriteData> goodMeteorites = new List<MeteoriteData>();
    public List<MeteoriteData> secretMeteorites = new List<MeteoriteData>();
    public List<GameObject> powerups = new List<GameObject>();
    private List<GameObject> allMeteorites = new List<GameObject>();
    public GameObject test;

    public GameObject explosion;
    public bool testPowerups;
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        Vector3 leftDown = new Vector3(0,0,20);
        Vector3 rightUp = new Vector3(1,1,20);
     
        Vector3 leftDownWorld = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 rightDownWorld = Camera.main.ViewportToWorldPoint(rightUp);
        bounds.x = leftDownWorld.x - 2;
        bounds.w = leftDownWorld.y - 2;
        bounds.z = rightDownWorld.y + 2;
        bounds.y = rightDownWorld.x + 2;
    }

    private void Start() {
        StartCoroutine(FirstMeteorite());
    }


    private IEnumerator FirstMeteorite() {
        for (int i = 0; i < maxAmount; i++) {
            NewMeteorite();
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        }
        yield return null;

        while (true) {
            yield return new WaitForSeconds(Random.Range(60, 90));
            NewMeteorite();
        }
    }


    public void NewMeteorite() {
        if (Random.Range(0, 100) < randomChance) {
            InstantiateRandomMeteorite();
        } else {
            InstantiateMeteorite();
        }
    }

    private void InstantiateMeteorite() {
        Vector4 playerRange = new Vector4(Player.position.x - range, Player.position.x + range, Player.position.y - range, Player.position.y + range);
        Vector2 goingTowards = new Vector2(Random.Range(playerRange.x, playerRange.y), Random.Range(playerRange.z, playerRange.w));

        float x = 0;
        float y = 0;
        if(Random.Range(0,100) > 50) {
            x = Random.Range(bounds.x, bounds.y);
            if (Random.Range(0, 100) > 50) {
                y = bounds.w;
            } else {
                y = bounds.z;
            }
        } else {
            y = Random.Range(bounds.z, bounds.w);
            if (Random.Range(0, 100) > 50) {
                x = bounds.x;
            } else {
                y = bounds.y;
            }
        }
        if (testPowerups) {
            CreatePowerUp(new Vector3(x, y, 0.4f), goingTowards);
            return;
        }
        MeteoriteData myRandomMeteorite = badMeteorites[Random.Range(0, badMeteorites.Count)];
        int r = Random.Range(0, 100);
        if (r < timeSinceLast / 2) {
            List<int> temp = h.GetNeeded();
            if (temp.Count != 0) {
                myRandomMeteorite = goodMeteorites[temp[Random.Range(0, temp.Count)]];
            }
            timeSinceLast = 0;
        }
        else if (Random.Range(0,100) < 5) {
            CreatePowerUp(new Vector3(x, y, 0.4f), goingTowards);
            return;
        }
        if(Random.Range(0,1000) < 2) {
            myRandomMeteorite = secretMeteorites[Random.Range(0, secretMeteorites.Count)];
        }
        GameObject newM = Instantiate(myRandomMeteorite.gObj, new Vector3(x, y,0.4f),Quaternion.identity);
        if (!myRandomMeteorite.isPositiveM) {
            allMeteorites.Add(newM);
        }

        newM.GetComponent<Meteorite>().Fill(myRandomMeteorite, goingTowards,new Vector2(minSpeed,maxSpeed));
    }

    private void InstantiateRandomMeteorite() {
        Vector2 goingTowards = new Vector2(Random.Range(bounds.x + 3, bounds.y - 3), Random.Range(bounds.z + 3, bounds.w - 3));

        float x = 0;
        float y = 0;
        if (Random.Range(0, 100) > 50) {
            x = Random.Range(bounds.x, bounds.y);
            if (Random.Range(0, 100) > 50) {
                y = bounds.w;
            } else {
                y = bounds.z;
            }
        } else {
            y = Random.Range(bounds.z, bounds.w);
            if (Random.Range(0, 100) > 50) {
                x = bounds.x;
            } else {
                y = bounds.y;
            }
        }
        if (testPowerups) {
            CreatePowerUp(new Vector3(x, y, 0.4f), goingTowards);
            return;
        }
        MeteoriteData myRandomMeteorite = badMeteorites[Random.Range(0, badMeteorites.Count)];
        int r = Random.Range(0, 100);
        if (r < timeSinceLast /2) {
            List<int> temp = h.GetNeeded();
            if(temp.Count != 0) {
                
                myRandomMeteorite = goodMeteorites[temp[Random.Range(0, temp.Count)]];
            }
            timeSinceLast = 0;
        } else if (Random.Range(0, 100) < 5) {
            CreatePowerUp(new Vector3(x, y, 0.4f), goingTowards);
            return;
        }
        if (Random.Range(0, 1000) < 2) {
            myRandomMeteorite = secretMeteorites[Random.Range(0, secretMeteorites.Count)];
        }
        GameObject newM = Instantiate(myRandomMeteorite.gObj, new Vector3(x, y, 0.4f), Quaternion.identity);
        if (!myRandomMeteorite.isPositiveM) {
            allMeteorites.Add(newM);
        }
        newM.GetComponent<Meteorite>().Fill(myRandomMeteorite, goingTowards, new Vector2(minSpeed, maxSpeed));
    }

    public void AddMeteor(GameObject met) {
        allMeteorites.Add(met);
    }

    private void CreatePowerUp(Vector3 loc,Vector3 going) {

        GameObject newM = Instantiate(powerups[Random.Range(0,powerups.Count)], loc, Quaternion.identity);
        newM.GetComponent<PowerUp>().Fill(going, new Vector2(minSpeed, maxSpeed));
    }

    public bool Inbounds(Vector3 loc){
        if (loc.x < bounds.y && loc.x > bounds.x && loc.y < bounds.z && loc.y > bounds.w) {
            return true;
        } else {
            return false;

        }
    }

    public void ExplodeThemAll() {
        StartCoroutine(ExplodeThemAllOnTurn());
    }

    private IEnumerator ExplodeThemAllOnTurn() {
        List<GameObject> allMNow = new List<GameObject>();
        allMNow.AddRange(allMeteorites);
        allMeteorites.Clear();
        for (int i = 0; i < allMNow.Count; i++) {
            if(allMNow[i] != null) {
                if(allMNow[i].GetComponent<Meteorite>() != null) {
                    allMNow[i].GetComponent<Meteorite>().Explode(true);
                }
                if(allMNow[i].GetComponent<MiniMeteorite>() != null) {
                    allMNow[i].GetComponent<MiniMeteorite>().Explode();
                }
                yield return null;
            }
        }
    }

    private bool slow;
    public PostProcessVolume ppv;
    public CinemachineVirtualCamera cam;
    private ChromaticAberration te;
    public void SlowTime(float time) {
        slow = true;

        StartCoroutine(SlowDisable(time));
    }

    IEnumerator SlowDisable(float time) {
        ppv.profile.TryGetSettings(out te);
        CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        while (te.intensity.value != 0.5f) {
            Debug.Log("towards");
            te.intensity.value = Mathf.MoveTowards(te.intensity.value, 0.5f,Time.deltaTime * 5);
            noise.m_AmplitudeGain = Mathf.MoveTowards(noise.m_AmplitudeGain, 0.5f, Time.deltaTime * 6);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        while (te.intensity.value != 0.07f) {
            Debug.Log("away");
            te.intensity.value = Mathf.MoveTowards(te.intensity.value, 0.07f, Time.deltaTime * 5);
            noise.m_AmplitudeGain = Mathf.MoveTowards(noise.m_AmplitudeGain, 0, Time.deltaTime * 6);
            yield return null;
        }
        slow = false;
    }
   
    public bool GetSlow() {
        return slow;
    }

    float timeSinceLast;
    private void Update() {
        timeSinceLast += Time.deltaTime;
    }
}




[System.Serializable]
public class MeteoriteData {
    public GameObject gObj;
    public GameObject smallGObj;
    public bool isPositiveM;
    public int id;
}
