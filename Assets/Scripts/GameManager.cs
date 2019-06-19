using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Player stuff
    public GameObject player;

    public float playerSpawnWaitTime = 3;
    private float playerTimeWaited;
    private bool playerWaiting;

    // Level system
    public string[] levelList;
    public bool gameCanEnd = true;

    private int currentLevel = 0;

    // Life system
    public float playerHitImmunityTime = 1f;
    public int playerMaxLives = 3;
    public LifePanel lifePanel;

    private float lastHitTime = float.NegativeInfinity;
    private int playerCurrentLives;

    // UI
    public Canvas canvas;
    public EventSystem eventSystem;

    public MenuScript pauseMenuPrefab;
    public MenuScript nextLevelMenuPrefab;
    public GameOverMenu gameOverMenuPrefab;

    public RectTransform drunkIndicator;
    private float indicatorOrigHeight;

    public TextMeshProUGUI levelText;

    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentLives = playerMaxLives;

        lifePanel.SetMaxLives(playerMaxLives);

        if (levelList.Length == 0)
        {
            Utils.Error("GameManager: Level List must have at least one level");
            return;
        }

        SceneManager.LoadScene(levelList[currentLevel], LoadSceneMode.Additive);

        if (drunkIndicator)
        {
            indicatorOrigHeight = drunkIndicator.sizeDelta.y;
        }

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWaiting)
        {
            if (playerTimeWaited < playerSpawnWaitTime)
            {
                playerTimeWaited += Time.deltaTime;
            }
            else
            {
                player.GetComponent<BoatMovement>().isPaused = false;
            }
        }
    }

    private void Reset()
    {
        playerTimeWaited = 0;
        playerWaiting = true;
        var levelCount = levelList.Length;
        var drunk = levelCount > 1 ? currentLevel * 1f / (levelList.Length - 1) : 1;
        player.GetComponent<BoatMovement>().drunkLevel = drunk;
        player.GetComponent<BoatMovement>().Reset();

        if (drunkIndicator)
        {
            var size = drunkIndicator.sizeDelta;
            size.y = indicatorOrigHeight * (1 - drunk);
            drunkIndicator.sizeDelta = size;
        }

        if (levelText)
        {
            levelText.text = $"Level {currentLevel + 1}/{levelCount}";
        }

        Resume();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Pause();
        playerCurrentLives = playerMaxLives;
        lifePanel.SetMaxLives(playerMaxLives);
        StartCoroutine(LoadLevelAsync(0));
    }

    public void NextLevel()
    {
        Pause();
        StartCoroutine(LoadLevelAsync((currentLevel + 1) % levelList.Length));
    }

    private IEnumerator LoadLevelAsync(int nextLevel)
    {
        var asUnload = SceneManager.UnloadSceneAsync(levelList[currentLevel]);
        currentLevel = nextLevel;
        var asLoad = SceneManager.LoadSceneAsync(levelList[currentLevel], LoadSceneMode.Additive);

        while (!asUnload.isDone || !asLoad.isDone)
        {
            yield return null;
        }

        Reset();
    }

    public void PlayerTakeDamage()
    {
        if (lastHitTime + playerHitImmunityTime > Time.time)
        {
            return;
        }

        Debug.Log("Player took damage!");
        playerCurrentLives--;

        lastHitTime = Time.time;

        lifePanel.SetCurrentLives(playerCurrentLives);

        if (playerCurrentLives == 0)
        {
            OnDeath();
        }
    }

    public void OnPauseMenu()
    {
        if (paused) { return; }

        paused = true;
        Pause();

        var menu = Instantiate(pauseMenuPrefab, canvas.transform);
        eventSystem.SetSelectedGameObject(menu.GetComponentInChildren<Button>().gameObject);
    }

    public void OnNextLevelMenu()
    {
        if (currentLevel == levelList.Length - 1)
        {
            // Last level, win!
            OnWin();
            return;
        }

        Pause();

        var menu = Instantiate(nextLevelMenuPrefab, canvas.transform);
        eventSystem.SetSelectedGameObject(menu.GetComponentInChildren<Button>().gameObject);
    }

    void OnDeath()
    {
        if (!gameCanEnd) return;

        Pause();

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);
        eventSystem.SetSelectedGameObject(gameOver.GetComponentInChildren<Button>().gameObject);

        gameOver.Lost();
    }

    public void OnWin()
    {
        Pause();

        if (!gameCanEnd)
        {
            Restart();
        }

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);
        eventSystem.SetSelectedGameObject(gameOver.GetComponentInChildren<Button>().gameObject);

        gameOver.Won();
    }
}
