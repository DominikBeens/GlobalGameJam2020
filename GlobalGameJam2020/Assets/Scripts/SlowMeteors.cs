using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMeteors : PowerUp
{
    public override void Use() {
        MeteoritesManager.instance.SlowTime(5);

    }
}
