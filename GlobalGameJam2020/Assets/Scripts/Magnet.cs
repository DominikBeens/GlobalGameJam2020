using System;
using UnityEngine;

public enum MagnetState { Push, Pull }

public class Magnet : MonoBehaviour {

    public static event Action<MagnetState> OnMagnetStateChanged = delegate { };
    public static event Action OnMagnetShot = delegate { };

    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private MagnetRay magnetRayPrefab;
    [SerializeField] private Transform shootPoint;

    private float shootTimer;
    private MagnetState magnetState;
    private LookAtMouse[] lookAts;

    private void Awake() {
        lookAts = GetComponentsInChildren<LookAtMouse>();
    }

    private void Start() {
        OnMagnetStateChanged(magnetState);
    }

    private void Update() {
        HandleMagnetSwitching();
        HandleShooting();
    }

    private void HandleMagnetSwitching() {
        if (Input.GetButtonDown("Jump")) {
            if (magnetState == MagnetState.Pull) {
                magnetState = MagnetState.Push;
            } else {
                magnetState = MagnetState.Pull;
            }
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
        } else {
            ToggleLookAts(true);
        }
    }

    private void Shoot() {
        MagnetRay magnetRay = Instantiate(magnetRayPrefab, shootPoint.position, shootPoint.rotation);
        magnetRay.Initialize(magnetState);
        OnMagnetShot();
    }

    private void ToggleLookAts(bool toggle) {
        foreach (LookAtMouse lookAt in lookAts) {
            lookAt.SetLookAt(toggle);
        }
    }
}
