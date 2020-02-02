using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiMoveie : PowerUp {
    public static bool InfiniMove = false;
    public float timer;
    public bool used;
    public override void Use() {
        InfiniMove = true;
        timer = 5;
        used = true;
    }

    void Update() {
        if (used) {
            if (timer > 0) {
                timer -= Time.deltaTime;
                if (timer < 0) {
                    InfiniMove = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}