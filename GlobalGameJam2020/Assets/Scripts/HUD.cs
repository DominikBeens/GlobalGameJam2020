using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour {

    [SerializeField] private Image magnetStateBackground;
    [SerializeField] private Image magnetStateFill;
    [SerializeField] private List<MagnetStateImage> magnetStateImages = new List<MagnetStateImage>();
    [Space]
    [SerializeField] private Image playerMoveCooldownFill;

    [System.Serializable]
    private struct MagnetStateImage {
        public GameObject GameObject;
        public MagnetState State;
        public Color Color;
        public Color BackgroundColor;
    }

    private void Awake() {
        Magnet.OnMagnetStateChanged += OnMagnetStateChangedHandler;
    }

    private void OnDestroy() {
        Magnet.OnMagnetStateChanged -= OnMagnetStateChangedHandler;
    }

    private void OnMagnetStateChangedHandler(MagnetState state) {
        magnetStateImages.ForEach(x => x.GameObject.SetActive(false));
        MagnetStateImage stateData = magnetStateImages.Find(x => x.State == state);
        magnetStateBackground.color = stateData.BackgroundColor;
        magnetStateFill.color = stateData.Color;
        stateData.GameObject.SetActive(true);
        stateData.GameObject.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    }
}
