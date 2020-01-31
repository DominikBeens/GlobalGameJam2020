using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private const string GAME_SCENE_NAME = "Game";

    [SerializeField] private Button playButton;

    private void Awake() {
        playButton.onClick.AddListener(PlayGame);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(PlayGame);
    }

    private void PlayGame() {
        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        });
    }
}
