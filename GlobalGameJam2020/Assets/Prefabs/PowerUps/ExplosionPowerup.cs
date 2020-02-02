using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPowerup : PowerUp
{
    public override void Use() {
        MeteoritesManager.instance.ExplodeThemAll();
        Destroy(gameObject);
    }
}
