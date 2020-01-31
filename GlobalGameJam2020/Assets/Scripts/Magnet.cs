using System;
using UnityEngine;

public enum MagnetState { Push, Pull }

public class Magnet : MonoBehaviour {

    public static event Action<MagnetState> OnMagnetStateChanged = delegate { };

    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private MagnetRay magnetRayPrefab;
    [SerializeField] private Transform shootPoint;

    private float shootTimer;
    private MagnetState magnetState;

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
        }
    }

    private void HandleShooting() {
        if (shootTimer > 0) {
            shootTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0)) {
            shootTimer = shootCooldown;
            Shoot();
        }
    }

    private void Shoot() {
        MagnetRay magnetRay = Instantiate(magnetRayPrefab, shootPoint.position, shootPoint.rotation);
        magnetRay.Initialize(magnetState);
    }
}
