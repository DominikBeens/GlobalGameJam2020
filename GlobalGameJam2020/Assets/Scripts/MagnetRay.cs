using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MagnetRay : MonoBehaviour {

    [SerializeField] private float force = 150f;
    [SerializeField] private float forceDeceleration = 0.01f;
    [SerializeField] private Transform rayHolder;
    [SerializeField] private float pushPullForce = 150f;

    [Header("Animation")]
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float scaleInDuration = 0.3f;
    [SerializeField] private float scaleOutDuration = 5f;
    [SerializeField] private float targetIdleScale = 0.6f;
    [SerializeField] private float idleStartDelay = 0.1f;
    [SerializeField] private float targetIdleWobbleScale = 0.8f;
    [SerializeField] private float targetIdleWobbleSpeed = 0.15f;
    [SerializeField] private float idleWobbleInterval = 0.5f;

    private new Rigidbody rigidbody;
    private MagnetState magnetState;
    private bool initialized;
    private float currentForce;
    private List<Transform> rayObjects = new List<Transform>();
    private float nextWobble;

    public void Initialize(MagnetState magnetState) {
        this.magnetState = magnetState;
        rigidbody = GetComponent<Rigidbody>();
        currentForce = force;
        initialized = true;
        nextWobble = Mathf.Infinity;
        StartCoroutine(StartAnimation());
    }

    private void Update() {
        if (!initialized) { return; }
        rigidbody.velocity = transform.up * currentForce;
        currentForce -= forceDeceleration;

        if (currentForce <= 1f) {
            Destroy(gameObject);
        }

        HandleWobble();
    }

    private void OnTriggerEnter(Collider other) {
        Meteorite meteorite = other.GetComponent<Meteorite>();
        if (!meteorite) { return; }
        Vector3 direction = magnetState == MagnetState.Push ? transform.forward : -transform.forward;
        meteorite.SetDirection(direction, pushPullForce);
        Destroy(gameObject);
    }

    private IEnumerator StartAnimation() {
        foreach (Transform ray in rayHolder) {
            rayObjects.Add(ray);
            ray.DOScaleX(0f, 0f);
            ray.gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        float lastStartDuration = 0f;
        for (int i = 0; i < rayObjects.Count; i++) {
            Transform ray = rayObjects[i];
            float delay = startDelay * i;
            ray.DOScaleX(1f, scaleInDuration).SetDelay(delay).SetEase(Ease.OutBack).OnStart(() => { 
                ray.gameObject.SetActive(true);
            }).OnComplete(() => {
                ray.DOScaleX(targetIdleScale, scaleOutDuration);
            });
        }

        if (magnetState == MagnetState.Pull) {
            rayObjects.Reverse();
        }

        yield return new WaitForSeconds(lastStartDuration);
        nextWobble = Time.time;
    }

    private void HandleWobble() {
        if (Time.time < nextWobble) { return; }
        nextWobble = Time.time + idleWobbleInterval;

        for (int i = 0; i < rayObjects.Count; i++) {
            Transform ray = rayObjects[i];
            ray.DOScaleX(targetIdleWobbleScale, targetIdleWobbleSpeed).SetDelay(idleStartDelay * i).OnComplete(() => {
                ray.DOScaleX(targetIdleScale, targetIdleWobbleSpeed);
            });
        }
    }
}
