using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HUD : MonoBehaviour {

    [SerializeField] private Transform container;
    [Space]
    [SerializeField] private Image magnetStateBackground;
    [SerializeField] private Image magnetStateFill;
    [SerializeField] private Image shieldFill;
    [SerializeField] private List<MagnetStateImage> magnetStateImages = new List<MagnetStateImage>();
    [SerializeField] private List<Image> colorableImages = new List<Image>();
    [Space]
    [SerializeField] private Image playerMoveCooldownFill;
    [SerializeField] private Color fullPlayerMoveCooldownColor;
    [SerializeField] private Color emptyPlayerMoveCooldownColor;
    [SerializeField] private float playerMoveCooldownShakeStrength = 5f;
    [SerializeField] private TextMeshProUGUI playerMoveCooldownText;

    private float previousCharge;
    private bool shaking;
    private MagnetStateImage currentStateImage;
    private float currentShieldCooldown;
    private float shieldCooldown;

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
        Magnet.OnShieldActivated += OnShieldActivatedHandler;
        Movement.OnChangeChanged += OnChangeChangedHandler;
    }

    private void OnDestroy() {
        Magnet.OnMagnetStateChanged -= OnMagnetStateChangedHandler;
        Magnet.OnMagnetShot -= OnMagnetShotHandler;
        Magnet.OnShieldActivated -= OnShieldActivatedHandler;
        Movement.OnChangeChanged -= OnChangeChangedHandler;
    }

    private void Update() {
        string moveCooldownText = (playerMoveCooldownFill.fillAmount * 100).ToString("F0");
        playerMoveCooldownText.text = $"{moveCooldownText}%";

        if (currentShieldCooldown < 1) {
            currentShieldCooldown += 1 / shieldCooldown * Time.deltaTime;
            shieldFill.DOFillAmount(currentShieldCooldown, 0.1f);
        }
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

        colorableImages.ForEach(x => x.color = state == MagnetState.Push ? Colors.Green01 : Colors.Red01);
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

    private void OnShieldActivatedHandler(float cooldown) {
        shieldCooldown = cooldown;
        currentShieldCooldown = 0;
    }
}
