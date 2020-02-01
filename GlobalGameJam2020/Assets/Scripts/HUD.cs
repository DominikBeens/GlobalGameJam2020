using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour {

    [SerializeField] private Transform container;
    [Space]
    [SerializeField] private Image magnetStateBackground;
    [SerializeField] private Image magnetStateFill;
    [SerializeField] private List<MagnetStateImage> magnetStateImages = new List<MagnetStateImage>();
    [Space]
    [SerializeField] private Image playerMoveCooldownFill;
    [SerializeField] private Color fullPlayerMoveCooldownColor;
    [SerializeField] private Color emptyPlayerMoveCooldownColor;
    [SerializeField] private float playerMoveCooldownShakeStrength = 5f;

    private float previousCharge;
    private bool shaking;
    private MagnetStateImage currentStateImage;

    [System.Serializable]
    private struct MagnetStateImage {
        public GameObject GameObject;
        public MagnetState State;
        public Color Color;
        public Color BackgroundColor;
    }

    private void Awake() {
        Magnet.OnMagnetStateChanged += OnMagnetStateChangedHandler;
        Magnet.OnMagnetShot += OnMagnetShotHandler;
        Movement.OnChangeChanged += OnChangeChangedHandler;
    }

    private void OnDestroy() {
        Magnet.OnMagnetStateChanged -= OnMagnetStateChangedHandler;
        Magnet.OnMagnetShot += OnMagnetShotHandler;
        Movement.OnChangeChanged -= OnChangeChangedHandler;
    }

    private void OnMagnetStateChangedHandler(MagnetState state) {
        magnetStateImages.ForEach(x => {
            x.GameObject.transform.DOKill(true);
            x.GameObject.SetActive(false); 
        });
        currentStateImage = magnetStateImages.Find(x => x.State == state);
        magnetStateBackground.color = currentStateImage.BackgroundColor;
        magnetStateFill.color = currentStateImage.Color;
        currentStateImage.GameObject.SetActive(true);
        currentStateImage.GameObject.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    }

    private void OnMagnetShotHandler() {
        currentStateImage.GameObject.transform.DOKill(true);
        currentStateImage.GameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
    }

    private void OnChangeChangedHandler(float charge) {
        playerMoveCooldownFill.fillAmount = charge;
        playerMoveCooldownFill.color = Color.Lerp(emptyPlayerMoveCooldownColor, fullPlayerMoveCooldownColor, playerMoveCooldownFill.fillAmount);
        if (charge < previousCharge && !shaking) {
            shaking = true;
            container.DOShakePosition(0.05f, playerMoveCooldownShakeStrength).OnComplete(() => { 
                shaking = false;
            });
        }
        previousCharge = charge;
    }
}
