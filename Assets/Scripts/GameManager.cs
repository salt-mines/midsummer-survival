using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playerHitImmunityTime = 1f;
    public int playerMaxLives = 3;

    public LifePanel lifePanel;

    private int playerCurrentLives;

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
        Debug.Log("Player died!");
    }

    public void OnWin()
    {
        Debug.Log("Player won!");
    }
}
