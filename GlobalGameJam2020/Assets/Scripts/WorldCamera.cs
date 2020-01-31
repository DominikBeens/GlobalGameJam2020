using UnityEngine;
using Cinemachine;

public class WorldCamera : MonoBehaviour {

    [SerializeField] private float speed = 10f;
    [SerializeField] private float scale = 1f;

    private CinemachineCameraOffset cameraOffset;

    private Camera mainCam;

    private void Awake() {
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        mainCam = Camera.main;
    }

    private void Update() {
        Vector3 offset = mainCam.ScreenToViewportPoint(Input.mousePosition);
        Vector3 centeredOffset = offset - new Vector3(0.5f, 0.5f, 0f);
        centeredOffset = new Vector3(centeredOffset.x * scale, centeredOffset.y * scale, centeredOffset.z * scale);
        cameraOffset.m_Offset = Vector3.Lerp(cameraOffset.m_Offset, centeredOffset, Time.deltaTime * speed);
    }
}
