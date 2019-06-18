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
