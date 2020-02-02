using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    public static Game Instance { get; private set; }

    [Header("Victory")]
    [SerializeField] private List<Color> sparkColors = new List<Color>();
    [SerializeField] private GameObject sparksPrefab;
    [SerializeField] private int victoryExplosions = 20;
    [SerializeField] private float victoryExplosionInterval = 0.2f;

    [Header("Defeat")]
    [SerializeField] private int defeatExplosions = 8;
    [SerializeField] private float defeatExplosionStartRange = 1.5f;
    [SerializeField] private float defeatExplosionInterval = 0.3f;

    private Coroutine victoryRoutine;
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
    }

    public void Victory() {
        if (victoryRoutine != null) { return; }
        victoryRoutine = StartCoroutine(VictoryRoutine());
    }

    public void Defeat() {
        if (defeatRoutine != null) { return; }
        defeatRoutine = StartCoroutine(DefeatRoutine());
    }

    private IEnumerator VictoryRoutine() {
        Movement movement = FindObjectOfType<Movement>();
        if (movement) {
            movement.canMove = false;
            Vector3 player = movement.transform.position;
            for (int i = 0; i < victoryExplosions; i++) {
                Vector2 randomInCircle = Random.insideUnitCircle * 10;
                Vector3 position = new Vector3(randomInCircle.x, randomInCircle.y, player.z - 5);
                Color randomColor = sparkColors[Random.Range(0, sparkColors.Count - 1)];
                GameObject explosion = Instantiate(sparksPrefab, position, Quaternion.Euler(0, 0, Random.value * 360));
                explosion.GetComponent<SpriteRenderer>().color = randomColor;
                explosion.transform.localScale = Vector3.one * Random.Range(0.8f, 1.5f);
                float interval = Random.Range(0.5f * victoryExplosionInterval, 1.2f * victoryExplosionInterval);
                yield return new WaitForSeconds(interval);
            }
        }

        yield return new WaitForSeconds(0.1f);
        ScreenFader.Instance.TogglePanel(ScreenFader.Panels.Victory);
        ScreenFader.Instance.FadeIn(1, () => {
            SceneManager.LoadScene("Menu");
        }, true);
    }

    private IEnumerator DefeatRoutine() {
        Movement movement = FindObjectOfType<Movement>();
        if (movement) {
            movement.canMove = false;
            Vector3 player = movement.transform.position;
            ExplodePlayer(movement);
            for (int i = 0; i < defeatExplosions; i++) {
                Vector2 randomInCircle = Random.insideUnitCircle;
                float range = defeatExplosionStartRange + (i + 0.5f);
                Vector3 position = player + new Vector3(randomInCircle.x * range, randomInCircle.y * range, player.z - 5);
                GameObject explosion = Instantiate(MeteoritesManager.instance.explosion, position, Quaternion.Euler(0, 0, Random.value * 360));
                explosion.transform.localScale = Vector3.one * (randomInCircle.x + 1);
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

    private void ExplodePlayer(Movement movement) {
        GameObject firstExplosion = Instantiate(MeteoritesManager.instance.explosion, movement.transform.position, Quaternion.Euler(0, 0, Random.value * 360));
        firstExplosion.transform.localScale = Vector3.one * 3f;

        movement.gameObject.SetActive(false);
        Transform characterJointBase = movement.GetComponent<SortOfParent>().child;
        characterJointBase.DOPunchPosition(Vector3.up * 2, 0.2f).OnComplete(() => { 
            characterJointBase.gameObject.SetActive(false);
        });
    }
}
