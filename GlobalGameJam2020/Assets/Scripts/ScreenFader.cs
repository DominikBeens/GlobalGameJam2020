using UnityEngine;
using DG.Tweening;
using System;

public class ScreenFader : MonoBehaviour {

    public static ScreenFader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool fadeOutOnAwake = true;

    private bool fading;

    private void Awake() {
        Instance = this;

        if (fadeOutOnAwake) {
            canvasGroup.DOFade(1, 0);
            FadeOut();
        } else {
            FadeOut(0);
        }
    }

    public void FadeIn(float speed = 1f, Action onComplete = null) {
        Fade(1, speed, onComplete);
    }

    public void FadeOut(float speed = 1f, Action onComplete = null) {
        Fade(0, speed, onComplete);
    }

    private void Fade(float alpha, float speed = 1f, Action onComplete = null) {
        if (fading) { return; }
        fading = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(alpha, speed).OnComplete(() => {
            onComplete?.Invoke();
            fading = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
}
