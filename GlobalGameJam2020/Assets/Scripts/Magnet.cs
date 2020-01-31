using System;
using UnityEngine;

public enum MagnetState { Push, Pull }

public class Magnet : MonoBehaviour {

    public static event Action<MagnetState> OnMagnetStateChanged = delegate { };

    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private MagnetRay magnetRayPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LookAtMouse lookAt;

    private float shootTimer;
    private MagnetState magnetState;

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
            lookAt.SetLookAt(false);
            shootTimer = shootCooldown;
            Shoot();
        } else {
            lookAt.SetLookAt(true);
        }
    }

    private void Shoot() {
        MagnetRay magnetRay = Instantiate(magnetRayPrefab, shootPoint.position, shootPoint.rotation);
        magnetRay.Initialize(magnetState);
    }
}
