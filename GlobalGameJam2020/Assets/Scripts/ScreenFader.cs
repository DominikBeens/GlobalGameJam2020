using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;

public class ScreenFader : MonoBehaviour {

    public static ScreenFader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool fadeOutOnAwake = true;
    [SerializeField] private CanvasGroup footer;
    [SerializeField] private List<Panel> panels = new List<Panel>();

    private bool fading;
    private Action onSpacePressed;
    private bool canPressSpace;

    public enum Panels { HowToPlay, Victory, Defeat }
    [Serializable]
    private class Panel {
        public CanvasGroup CanvasGroup;
        public Panels PanelType;
    }

    private void Awake() {
        Instance = this;

        if (fadeOutOnAwake) {
            canvasGroup.DOFade(1, 0);
            FadeOut();
        } else {
            FadeOut(0);
        }
        DisableAllPanels();
    }

    private void Update() {
        if (!canPressSpace) { return; }
        if (Input.GetButtonDown("Jump")) {
            onSpacePressed?.Invoke();
            canPressSpace = false;
        }
    }

    public void TogglePanel(Panels panelType, float speed = 0.5f, float delay = 0.5f) {
        DisableAllPanels();
        Panel panel = panels.FirstOrDefault(x => x.PanelType == panelType);
        panel?.CanvasGroup.DOFade(1, speed).SetDelay(delay);
    }

    public void FadeIn(float speed = 1f, Action onComplete = null, bool spaceToExecuteAction = false) {
        Fade(1, speed, onComplete, spaceToExecuteAction);
    }

    public void FadeOut(float speed = 1f, Action onComplete = null, bool spaceToExecuteAction = false) {
        Fade(0, speed, onComplete, spaceToExecuteAction);
    }

    private void Fade(float alpha, float speed = 1f, Action onComplete = null, bool spaceToExecuteAction = false) {
        if (fading) { return; }
        fading = true;
        canvasGroup.blocksRaycasts = true;

        if (spaceToExecuteAction) {
            footer.DOFade(1, speed / 2f);
        } else {
            footer.DOFade(0, speed / 2f);
        }

        canvasGroup.DOFade(alpha, speed).OnComplete(() => {
            if (spaceToExecuteAction) {
                onSpacePressed = onComplete;
                canPressSpace = true;
            } else {
                onComplete?.Invoke();
            }
            fading = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    private void DisableAllPanels() {
        panels.ForEach(x => x.CanvasGroup.DOFade(0, 0));
    }
}
