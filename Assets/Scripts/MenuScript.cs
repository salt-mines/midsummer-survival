using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void OnResume()
    {
        Destroy(gameObject);
        gameManager.Resume();
    }

    public void OnRestart()
    {
        Destroy(gameObject);
        gameManager.Restart();
    }

    public void OnNextLevel()
    {
        Destroy(gameObject);
        gameManager.NextLevel();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
