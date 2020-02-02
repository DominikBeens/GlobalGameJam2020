using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Shield : MonoBehaviour {

    [SerializeField] private float lifeTime = 5f;

    private Movement player;

    public void Initialize(Movement player) {
        this.player = player;
        AnimateIn();
        AnimateOut();
    }

    private void Update() {
        if (!player) { return; }
        Vector3 position = player.transform.position;
        position.z += 1;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other) {
        Meteorite meteorite = other.GetComponent<Meteorite>();
        if (meteorite) {
            meteorite.Explode();
        }
        MiniMeteorite miniMeteorite = other.GetComponent<MiniMeteorite>();
        if (miniMeteorite) {
            Destroy(miniMeteorite.gameObject);
        }
    }

    private void AnimateIn() {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        transform.DOBlendableRotateBy(new Vector3(0, 0, 90), 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(() => {
            transform.DOBlendableRotateBy(new Vector3(0, 0, -360), lifeTime, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
            transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 5).SetEase(Ease.Linear).SetLoops(-1);
        });
    }

    private void AnimateOut() {
        StartCoroutine(AnimateOutRoutine());
    }

    private IEnumerator AnimateOutRoutine() {
        yield return new WaitForSeconds(lifeTime);
        transform.DOKill();
        transform.DOBlendableRotateBy(new Vector3(0, 0, 90), 0.2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
        transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
