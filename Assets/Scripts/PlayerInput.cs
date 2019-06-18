using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool Pause { get; private set; }

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Pause = Input.GetAxisRaw("Pause") > 0;

        if(Pause)
        {
            gameManager.OnPauseMenu();
        }
    }
}
