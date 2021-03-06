﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MagnetRay : MonoBehaviour {

    [SerializeField] private float force = 150f;
    [SerializeField] private float forceDeceleration = 0.01f;
    [SerializeField] private Transform rayHolder;
    [SerializeField] private float pushPullForce = 150f;

    [Header("Animation")]
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float scaleInDuration = 0.3f;
    [SerializeField] private float scaleOutDuration = 5f;
    [SerializeField] private float scaleInTargetScale = 1.5f;
    [SerializeField] private float targetIdleScale = 0.6f;
    [SerializeField] private float idleStartDelay = 0.1f;
    [SerializeField] private float targetIdleWobbleScale = 0.8f;
    [SerializeField] private float targetIdleWobbleSpeed = 0.15f;
    [SerializeField] private float idleWobbleInterval = 0.5f;
    [SerializeField] AudioSource myAudioSource;

    private new Rigidbody rigidbody;
    private new Collider collider;
    private MagnetState magnetState;
    private bool initialized;
    private float currentForce;
    private List<Transform> rayObjects = new List<Transform>();
    private float nextWobble;
    private bool destroying;

    public void Initialize(MagnetState magnetState) {
        this.magnetState = magnetState;
        if (magnetState == MagnetState.Pull) {
            myAudioSource.pitch = 1f;
        } else {
            myAudioSource.pitch = 0.6f;
        }
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        collider.enabled = false;
        currentForce = force;
        initialized = true;
        nextWobble = Mathf.Infinity;
        foreach (MagnetRayRay ray in GetComponentsInChildren<MagnetRayRay>(true)) {
            ray.Initialize(magnetState);
        }

        StartCoroutine(EnableCollider());
        StartCoroutine(StartAnimation());
    }

    private void Update() {
        if (!initialized) { return; }
        rigidbody.velocity = transform.up * currentForce;
        currentForce -= forceDeceleration;

        if (currentForce <= 1f) {
            DestroyRay();
        }

        HandleWobble();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Shield>()) { return; }

        Meteorite meteorite = other.GetComponent<Meteorite>();
        if (meteorite) {
            Vector3 direction = magnetState == MagnetState.Push ? transform.up : -transform.up;
            meteorite.SetDirection(direction, pushPullForce);
        }
        MiniMeteorite miniMeteorite = other.GetComponent<MiniMeteorite>();
        if (miniMeteorite) {
            Vector3 direction = magnetState == MagnetState.Push ? transform.up : -transform.up;
            miniMeteorite.SetDirection(direction, pushPullForce);
        }
        PowerUp powerUp = other.GetComponent<PowerUp>();
        if (powerUp) {
            Vector3 direction = magnetState == MagnetState.Push ? transform.up : -transform.up;
            powerUp.SetDirection(direction, pushPullForce);
        }
        DestroyRay();
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
            ray.DOScaleX(scaleInTargetScale, scaleInDuration).SetDelay(delay).SetEase(Ease.OutBack).OnStart(() => {
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

    private void DestroyRay() {
        if (destroying) { return; }
        destroying = true;
        nextWobble = Mathf.Infinity;
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine() {
        foreach (Transform ray in rayObjects) {
            ray.DOKill();
        }

        Destroy(collider);

        if (magnetState == MagnetState.Pull) {
            rayObjects.Reverse();
        }

        for (int i = rayObjects.Count - 1; i >= 0; i--) {
            Transform ray = rayObjects[i];
            ray.DOScaleX(0, 0.1f).SetDelay(0.05f * i).OnComplete(() => {
                ray.gameObject.SetActive(false);
            });
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator EnableCollider() {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = true;
    }
}