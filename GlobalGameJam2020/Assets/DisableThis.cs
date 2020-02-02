using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableThis : MonoBehaviour {
    public void DisableNow() {
        gameObject.SetActive(false);
    }
}