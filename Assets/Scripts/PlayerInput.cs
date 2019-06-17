using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool Pause { get; private set; }
    
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Pause = Input.GetAxisRaw("Pause") > 0;
    }
}
