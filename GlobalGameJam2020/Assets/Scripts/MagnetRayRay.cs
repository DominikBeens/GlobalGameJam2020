using UnityEngine;
using DG.Tweening;

public class MagnetRayRay : MonoBehaviour {

    [SerializeField] private SpriteRenderer raySprite;
    [SerializeField] private Transform container;

    public void Initialize(MagnetState state) {
        raySprite.color = state == MagnetState.Pull ? Colors.Red01 : Colors.Green01;
        container.DOScaleX(Random.Range(0.9f, 1.1f), 0);
        if (Random.value < 0.5f) {
            container.DOScaleX(container.localScale.x * -1, 0);
        }
    }
}
