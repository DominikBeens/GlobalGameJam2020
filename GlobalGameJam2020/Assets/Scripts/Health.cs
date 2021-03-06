﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Health : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI counterText;

    public static Health Instance { get; private set; }

    private HealthIcon[] healthIcons;
    private bool killOnNextHit;

    private void Awake() {
        Instance = this;
        healthIcons = GetComponentsInChildren<HealthIcon>();
        foreach (HealthIcon icon in healthIcons) {
            icon.SetHealthIconOff();
        }
        killOnNextHit = true;
        UpdateText();
    }

    public void Increment(int spriteId) {
        HealthIcon icon = healthIcons.FirstOrDefault(x => x.SpriteId == spriteId);
        icon.SetHealthIconOn();
        killOnNextHit = false;
        UpdateText();

        if (healthIcons.All(x => x.IsActive)) {
            Game.Instance.Victory();
        }
    }

    public void Decrement() {
        if (killOnNextHit) {
            Game.Instance.Defeat();
            return;
        }

        List<HealthIcon> icons = new List<HealthIcon>();
        foreach (HealthIcon icon in healthIcons) {
            if (icon.IsActive) {
                icons.Add(icon);
            }
        }

        icons[Random.Range(0, icons.Count - 1)].SetHealthIconOff();

        if (healthIcons.FirstOrDefault(x => x.IsActive) == null) {
            killOnNextHit = true;
        }

        UpdateText();
    }

    private void UpdateText() {
        int active = 0;
        foreach (HealthIcon icon in healthIcons) {
            if (icon.IsActive) {
                active++;
            }
        }
        string suffix = killOnNextHit ? "<color=yellow>!</color>" : string.Empty;
        counterText.text = $"{active}/{healthIcons.Length}{suffix}";
        counterText.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    }
    public List<int> GetNeeded() {
        List<int> toBe = new List<int>();
        for (int i = 0; i < healthIcons.Length; i++) {
            if (!healthIcons[i].IsActive) {
                toBe.Add(healthIcons[i].SpriteId);
            }
        }
        return toBe;
    }
}
