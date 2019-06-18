﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Player stuff
    public GameObject player;
    private Vector3 playerPos;

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

    private int playerCurrentLives;

    // UI
    public Canvas canvas;

    public MenuScript pauseMenuPrefab;
    public MenuScript nextLevelMenuPrefab;
    public GameOverMenu gameOverMenuPrefab;

    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;
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
        player.transform.position = playerPos;
        playerTimeWaited = 0;
        player.GetComponent<BoatMovement>().isPaused = true;
        playerWaiting = true;

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
        Debug.Log("Player took damage!");
        playerCurrentLives--;

        lifePanel.SetCurrentLives(playerCurrentLives);

        if(playerCurrentLives == 0)
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

    public void OnNextLevelMenu()
    {
        if (currentLevel == levelList.Length - 1)
        {
            // Last level, win!
            OnWin();
            return;
        }

        Pause();

        Instantiate(nextLevelMenuPrefab, canvas.transform);
    }

    void OnDeath()
    {
        if (!gameCanEnd) return;

        Pause();
        Debug.Log("Player died!");

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);

        gameOver.Lost();
    }

    public void OnWin()
    {
        Pause();
        Debug.Log("Player won!");

        if (!gameCanEnd)
        {
            Restart();
        }

        GameOverMenu gameOver = Instantiate(gameOverMenuPrefab, canvas.transform);

        gameOver.Won();
    }
}
