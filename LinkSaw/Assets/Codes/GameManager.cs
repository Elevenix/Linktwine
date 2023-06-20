using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Monsters")]
    [SerializeField]
    private float posSpawnMonsters;
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private float timeChangeWaves = 20f;

    [Header("Score menu")]
    [SerializeField]
    private TextMeshPro scoreText;
    [SerializeField]
    private Animator transitionAnimator;
    [SerializeField]
    private Animator menu;
    [SerializeField]
    private GameObject scoreObject;
    [SerializeField]
    private TextMeshProUGUI scoreMenu;
    [SerializeField]
    private TextMeshProUGUI highScoreMenu;

    [Header("Player")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject particuleDeath;

    public static GameManager GM;

    private Camera _mainCamera;
    private int _actualScore = 0;
    private Animator _anim;
    private bool _isRunning = true;
    private int _actualWave = 0;
    private void Awake()
    {
        if(GM == null)
        {
            GM = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
        }
        if (!PlayerPrefs.HasKey("highScore"))
        {
            PlayerPrefs.SetInt("highScore", 0);
        }

        if (PlayerPrefs.GetInt("highScore") <= 0)
        {
            scoreObject.SetActive(false);
        }
        highScoreMenu.text = PlayerPrefs.GetInt("highScore").ToString();
        scoreMenu.text = PlayerPrefs.GetInt("score").ToString();

        _mainCamera = FindObjectOfType<Camera>();
        scoreText.text = "0";
        _anim = GetComponent<Animator>();
    }

    private IEnumerator spawnMonsters()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(delayChangeWave());
        while (_isRunning)
        {
            Wave w = waves[_actualWave];
            yield return new WaitForSeconds(Random.Range(w.minRandom,w.maxRandom));
            Vector2 randPosSpawn = Random.insideUnitCircle.normalized * posSpawnMonsters;
            GameObject randMonster = w.monsters[Random.Range(0, w.monsters.Length)];
            if(_isRunning)
                Instantiate(randMonster, randPosSpawn, Quaternion.identity);
        }
    }

    private IEnumerator delayChangeWave()
    {
        while(_actualWave < waves.Length-1)
        {
            yield return new WaitForSeconds(timeChangeWaves);
            _actualWave++;
        }
    }

    public void addScore(int score, float timeShake, float magnitudeShake)
    {
        StartCoroutine(Shake(timeShake, magnitudeShake));
        _anim.SetTrigger("add");
        _actualScore += score;
        scoreText.text = _actualScore.ToString();
    }

    public void endGame()
    {
        StartCoroutine(delayTransition());
        _isRunning = false;
        PlayerPrefs.SetInt("score", _actualScore);
        if(PlayerPrefs.GetInt("highScore") < _actualScore)
        {
            PlayerPrefs.SetInt("highScore", _actualScore);
        }
        Instantiate(particuleDeath, player.transform.position, Quaternion.identity);
        Destroy(player);
        StartCoroutine(Shake(0.2f, 0.5f));
    }

    IEnumerator delayTransition()
    {
        yield return new WaitForSecondsRealtime(2f);
        transitionAnimator.SetTrigger("transition");
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(0);
    }


    // screen shake (by Brackeys)
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = _mainCamera.transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _mainCamera.transform.localPosition = new Vector3(x, y, _mainCamera.transform.localPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.localPosition = originalPos;
    }

    public void startGame()
    {
        menu.SetTrigger("start");
        _mainCamera.GetComponent<Animator>().SetTrigger("start");
        Time.timeScale = 1;
        _isRunning = true;
        StartCoroutine(spawnMonsters());
    }

    public void quitApp()
    {
        Application.Quit();
    }
}
