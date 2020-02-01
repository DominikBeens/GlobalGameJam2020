using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    public static Game Instance { get; private set; }

    [SerializeField] private int defeatExplosions = 8;
    [SerializeField] private float defeatExplosionStartRange = 1.5f;
    [SerializeField] private float defeatExplosionInterval = 0.3f;

    private Coroutine defeatRoutine;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ScreenFader.Instance.FadeIn(1, () => {
                SceneManager.LoadScene("Menu");
            });
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            Defeat();
        }
    }

    public void Victory() {
        ScreenFader.Instance.TogglePanel(ScreenFader.Panels.Defeat);
        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene("Game");
        }, true);
    }

    public void Defeat() {
        if (defeatRoutine != null) { return; }
        defeatRoutine = StartCoroutine(DefeatRoutine());
    }

    private IEnumerator DefeatRoutine() {
        Movement movement = FindObjectOfType<Movement>();
        if (movement) {
            Vector3 player = movement.transform.position;
            for (int i = 0; i < defeatExplosions; i++) {
                Vector2 randomInCircle = Random.insideUnitCircle;
                float range = defeatExplosionStartRange + (i + 0.5f);
                Vector3 position = player + new Vector3(randomInCircle.x * range, randomInCircle.y * range, player.z - 5);
                GameObject explosion = Instantiate(MeteoritesManager.instance.explosion, position, Quaternion.Euler(0, 0, Random.value * 360));
                explosion.transform.localScale = Vector3.one * randomInCircle.x;
                float interval = Random.Range(0.8f * defeatExplosionInterval, 1.2f * defeatExplosionInterval);
                yield return new WaitForSeconds(interval);
            }
        }

        yield return new WaitForSeconds(0.1f);
        ScreenFader.Instance.TogglePanel(ScreenFader.Panels.Defeat);
        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene("Game");
        }, true);
    }
}
