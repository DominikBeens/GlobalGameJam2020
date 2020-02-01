using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour {

    private const string GAME_SCENE_NAME = "Game";

    [SerializeField] private Button playButton;
    [SerializeField] private Transform spaceShip;
    [SerializeField] private CanvasGroup liftoffTextCanvasGroup;
    [SerializeField] private TextMeshProUGUI liftoffText;

    private void Awake() {
        playButton.onClick.AddListener(PlayGame);
        liftoffTextCanvasGroup.DOFade(0, 0);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(PlayGame);
    }

    private void PlayGame() {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine() {
        playButton.interactable = false;
        spaceShip.DOShakePosition(20f, 0.5f, 5).SetDelay(2).SetEase(Ease.Linear);

        SetLiftoffText(3);
        yield return new WaitForSeconds(1f);
        SetLiftoffText(2);
        yield return new WaitForSeconds(1f);
        SetLiftoffText(1);
        yield return new WaitForSeconds(1f);
        SetLiftoffText(0);

        spaceShip.DOMove(spaceShip.position + (spaceShip.up * 500), 3f).SetEase(Ease.InSine);

        yield return new WaitForSeconds(0.5f);

        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        });
    }

    private void SetLiftoffText(int seconds) {
        liftoffTextCanvasGroup.DOFade(0, 0);
        liftoffText.text = $"T-00:0{seconds}";
        liftoffTextCanvasGroup.DOFade(1, 0.1f);
    }
}
