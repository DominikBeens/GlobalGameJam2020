using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public enum MagnetState { Push, Pull }

public class Magnet : MonoBehaviour {

    public static event Action<MagnetState> OnMagnetStateChanged = delegate { };
    public static event Action OnMagnetShot = delegate { };
    public static event Action<float> OnShieldActivated = delegate { };

    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float shieldCooldown = 10f;
    [SerializeField] private MagnetRay magnetRayPrefab;
    [SerializeField] private Shield shieldPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform magnetRayHead;
    [SerializeField] private GameObject pushArm;
    [SerializeField] private GameObject pullArm;
    [SerializeField] private GameObject sparks;

    private float shootTimer;
    private float shieldTimer;
    private MagnetState magnetState;
    private LookAtMouse[] lookAts;
    private Movement player;

    private void Awake() {
        lookAts = GetComponentsInChildren<LookAtMouse>();
        player = FindObjectOfType<Movement>();
        ToggleArmImage();
        sparks.SetActive(false);
    }

    private void Start() {
        OnMagnetStateChanged(magnetState);
    }

    private void Update() {
        HandleMagnetSwitching();
        HandleShooting();
        HandleShield();
    }

    private void HandleMagnetSwitching() {
        if (Input.GetButtonDown("Jump")) {
            if (magnetState == MagnetState.Pull) {
                magnetState = MagnetState.Push;
            } else {
                magnetState = MagnetState.Pull;
            }
            ToggleArmImage();
            OnMagnetStateChanged(magnetState);
        }
    }

    private void HandleShooting() {
        if (shootTimer > 0) {
            shootTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0)) {
            ToggleLookAts(false);
            shootTimer = shootCooldown;
            Shoot();
            sparks.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            sparks.transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
            sparks.SetActive(true);
        } else {
            ToggleLookAts(true);
            sparks.SetActive(false);
        }
    }

    private void HandleShield() {
        if (shieldTimer > 0) {
            shieldTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(1)) {
            shieldTimer = shieldCooldown;
            Shield();
        }
    }

    private void Shoot() {
        MagnetRay magnetRay = Instantiate(magnetRayPrefab, shootPoint.position, shootPoint.rotation);
        magnetRay.Initialize(magnetState);
        OnMagnetShot();
        magnetRayHead.DOPunchPosition(magnetRayHead.InverseTransformDirection(magnetRayHead.up) * 0.1f, 0.1f);
    }

    private void Shield() {
        Shield shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
        shield.Initialize(player);
        OnShieldActivated(shieldCooldown);
    }

    private void ToggleLookAts(bool toggle) {
        foreach (LookAtMouse lookAt in lookAts) {
            lookAt.SetLookAt(toggle);
        }
    }

    private void ToggleArmImage() {
        pushArm.SetActive(magnetState == MagnetState.Push);
        pullArm.SetActive(magnetState == MagnetState.Pull);
    }
}
