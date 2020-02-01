﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject test;

    public GameObject explosion;

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

        MeteoriteData myRandomMeteorite = badMeteorites[Random.Range(0, badMeteorites.Count)];
        int r = Random.Range(0, 100);
        if (r < 25) {
            List<int> temp = h.GetNeeded();
            if (temp.Count != 0) {
                myRandomMeteorite = goodMeteorites[temp[Random.Range(0, temp.Count)]];
            }
        }
        if(Random.Range(0,1000) < 2) {
            myRandomMeteorite = secretMeteorites[Random.Range(0, secretMeteorites.Count)];
        }
        GameObject newM = Instantiate(myRandomMeteorite.gObj, new Vector3(x, y,0.4f),Quaternion.identity);
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

        MeteoriteData myRandomMeteorite = badMeteorites[Random.Range(0, badMeteorites.Count)];
        int r = Random.Range(0, 100);
        if (r < 25) {

            List<int> temp = h.GetNeeded();
            if(temp.Count != 0) {
                
                myRandomMeteorite = goodMeteorites[temp[Random.Range(0, temp.Count)]];
            }
        } 
        GameObject newM = Instantiate(myRandomMeteorite.gObj, new Vector3(x, y, 0.4f), Quaternion.identity);
        newM.GetComponent<Meteorite>().Fill(myRandomMeteorite, goingTowards, new Vector2(minSpeed, maxSpeed));
    }

    public bool Inbounds(Vector3 loc){
        if (loc.x < bounds.y && loc.x > bounds.x && loc.y < bounds.z && loc.y > bounds.w) {
            return true;
        } else {
            return false;

        }
    }
}

[System.Serializable]
public class MeteoriteData {
    public GameObject gObj;
    public GameObject smallGObj;
    public bool isPositiveM;
    public int id;
}
