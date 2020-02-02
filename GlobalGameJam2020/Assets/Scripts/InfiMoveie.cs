using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiMoveie : PowerUp {
    public static bool InfiniMove = false;
    public float timer;

    public override void Use() {
        InfiniMove = true;
        timer = 5;
    }

    void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer < 0) {
                InfiniMove = false;
            }
        }
    }
}