using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class MainMenu : MonoBehaviour {

    private const string GAME_SCENE_NAME = "Game";

    [SerializeField] private Button playButton;
    [SerializeField] private Transform spaceShip;

    private void Awake() {
        playButton.onClick.AddListener(PlayGame);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(PlayGame);
    }

    private void PlayGame() {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine() {
        playButton.interactable = false;

        spaceShip.DOShakePosition(20f, 5f, 20).SetEase(Ease.Linear);
        spaceShip.DOShakeScale(1f, 0.1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        spaceShip.DOMove(spaceShip.position + (spaceShip.up * 7500), 5f).SetEase(Ease.InSine);

        yield return new WaitForSeconds(0.5f);

        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        });
    }
}
