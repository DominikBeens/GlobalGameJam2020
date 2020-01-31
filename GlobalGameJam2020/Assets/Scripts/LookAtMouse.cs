using UnityEngine;

public class LookAtMouse : MonoBehaviour {

    [SerializeField] private float speed = 20f;

    private void Update() {
        Vector3 toMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = (Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg) - 90;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
