﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    public static Game Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ScreenFader.Instance.FadeIn(1, () => {
                SceneManager.LoadScene("Menu");
            });
        }
    }

    public void GameOver() {
        SceneManager.LoadScene("Game");
    }
}
