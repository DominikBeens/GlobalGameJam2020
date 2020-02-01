using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthIcon : MonoBehaviour {

    [SerializeField] private Image healthImage;
    [SerializeField] private int spriteId;

    public bool IsActive { get; private set; }
    public int SpriteId => spriteId;

    public void SetHealthIconOn() {
        IsActive = true;
        healthImage.DOKill();
        healthImage.DOFade(1, 0.2f);
    }

    public void SetHealthIconOff() {
        IsActive = false;
        healthImage.DOKill();
        healthImage.DOFade(0.2f, 0.2f);
    }
}
