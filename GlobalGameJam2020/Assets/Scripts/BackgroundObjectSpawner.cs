using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BackgroundObjectSpawner : MonoBehaviour {

    [Serializable]
    private class BackgroundObject {
        public GameObject Prefab;
        public float MinScale = 0.8f;
        public float MaxScale = 1.3f;
        public float MinZPosition = -10f;
        public float MaxZPosition = -5f;
        public int SpawnAmount = 10;
        public float ClearRange = 2.5f;
    }

    [SerializeField] private List<BackgroundObject> backgroundObjects = new List<BackgroundObject>();

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Camera mainCam;

    private void Awake() {
        mainCam = Camera.main;
        SpawnBackgroundObjects();
    }

    private void SpawnBackgroundObjects() {
        foreach (BackgroundObject obj in backgroundObjects) {
            for (int i = 0; i < obj.SpawnAmount; i++) {
                bool foundValidPosition = false;
                int tries = 25;
                while (!foundValidPosition && tries > 0) {
                    Vector3 position = GetRandomPosition(obj);
                    if (IsValidPosition(position, obj.ClearRange)) {
                        foundValidPosition = true;
                        SpawnObject(obj, position);
                    }
                    tries--;
                }
            }
        }
    }

    private Vector3 GetRandomPosition(BackgroundObject obj) {
        float viewportX = Random.Range(-0.1f, 1.1f);
        float viewportY = Random.Range(-0.1f, 1.1f);
        Vector3 worldPosition = mainCam.ViewportToWorldPoint(new Vector3(viewportX, viewportY, 20));
        worldPosition.z = Random.Range(obj.MinZPosition, obj.MaxZPosition);
        return worldPosition;
    }

    private bool IsValidPosition(Vector3 position, float allowedRange) {
        if (spawnedObjects.Count == 0) { return true; }
        foreach (GameObject spawnedObject in spawnedObjects) {
            float distance = (spawnedObject.transform.position - position).sqrMagnitude;
            if (distance < (allowedRange * allowedRange)) {
                return false;
            }
        }
        return true;
    }

    private void SpawnObject(BackgroundObject obj, Vector3 position) {
        GameObject newObject = Instantiate(obj.Prefab, position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        newObject.transform.SetParent(transform);
        spawnedObjects.Add(newObject);
    }
}
