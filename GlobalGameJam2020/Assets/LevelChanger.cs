using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public void ChangeLevel() {
        ScreenFader.Instance.TogglePanel(ScreenFader.Panels.HowToPlay);
        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene("Game");
        }, true);
    }
}