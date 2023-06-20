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
        // set the data 
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

    /// <summary>
    /// Loop spawn monsters
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Loop to change the actual wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator delayChangeWave()
    {
        while(_actualWave < waves.Length-1)
        {
            yield return new WaitForSeconds(timeChangeWaves);
            _actualWave++;
        }
    }

    /// <summary>
    /// Add score and screen shake when score get (when a monster is killed)
    /// </summary>
    /// <param name="score"> score get </param>
    /// <param name="timeShake"> time of shake </param>
    /// <param name="magnitudeShake"> magnitude of shake </param>
    public void addScore(int score, float timeShake, float magnitudeShake)
    {
        StartCoroutine(Shake(timeShake, magnitudeShake));
        _anim.SetTrigger("add");
        _actualScore += score;
        scoreText.text = _actualScore.ToString();
    }

    /// <summary>
    /// Launch the end of the game, when player die
    /// </summary>
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

    /// <summary>
    /// Coroutine of scene transition to reload the level
    /// </summary>
    /// <returns></returns>
    IEnumerator delayTransition()
    {
        yield return new WaitForSecondsRealtime(2f);
        transitionAnimator.SetTrigger("transition");
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// Screen shake method (created by Brackeys)
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
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

    /// <summary>
    /// When the game start
    /// </summary>
    public void startGame()
    {
        menu.SetTrigger("start");
        _mainCamera.GetComponent<Animator>().SetTrigger("start");
        Time.timeScale = 1;
        _isRunning = true;
        StartCoroutine(spawnMonsters());
    }

    /// <summary>
    /// to quit the app
    /// </summary>
    public void quitApp()
    {
        Application.Quit();
    }
}
