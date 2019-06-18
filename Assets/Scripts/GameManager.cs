using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Level system
    public string[] levelList;

    private int currentLevel = 0;

    // Life system
    public float playerHitImmunityTime = 1f;
    public int playerMaxLives = 3;

    public LifePanel lifePanel;

    private int playerCurrentLives;

    public Canvas canvas;

    public MenuScript pauseMenuPrefab;
    public GameObject nextLevelMenuPrefab;
    public GameOverMenu gameOverMenuPrefab;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }

    public void NextLevel()
    {

    }

    public void PlayerTakeDamage()
    {
        Debug.Log("Player took damage!");
        playerCurrentLives--;

        lifePanel.SetCurrentLives(playerCurrentLives);

        if(playerCurrentLives < 0)
        {
            OnDeath();
        }
    }

    public void OnPauseMenu()
    {
        if(paused) { return; }

        Pause();

        Instantiate(pauseMenuPrefab, canvas.transform);

        paused = true;

    }

    void OnDeath()
    {
        Pause();
        Debug.Log("Player died!");

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);

        gameOver.Lost();
    }

    public void OnWin()
    {
        Pause();
        Debug.Log("Player won!");

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);

        gameOver.Won();
    }
}
