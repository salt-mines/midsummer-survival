using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playerHitImmunityTime = 1f;
    public int playerMaxLives = 3;

    public LifePanel lifePanel;

    private int playerCurrentLives;

    public Canvas canvas;

    public GameObject pauseMenuPrefab;
    public GameObject nextLevelMenuPrefab;
    public GameOverMenu gameOverMenuPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentLives = playerMaxLives;

        lifePanel.SetMaxLives(playerMaxLives);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Time.timeScale = 1;
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
