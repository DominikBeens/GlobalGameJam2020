using UnityEngine;

public class MagnetRay : MonoBehaviour {

    [SerializeField] private float force = 10f;
    [SerializeField] private float destroyAfterHitTime = 0.2f;

    private MagnetState magnetState;

    public void Initialize(MagnetState magnetState) {
        this.magnetState = magnetState;
    }

    private void OnTriggerEnter(Collider other) {
        //Meteorite meteorite = other.GetComponent<Meteorite>();
        //if (!meteorite) { return; }
        //Vector3 direction = magnetState == MagnetState.Push ? transform.forward : -transform.forward;
        //meteorite.rigidbody.AddForce(direction * force);
        Destroy(gameObject, destroyAfterHitTime);
    }
}
