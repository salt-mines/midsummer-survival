using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class BoatMovement : MonoBehaviour
{
    public float boatSpeed = 6f;
    public float boatStrafeSpeed = 3f;

    private PlayerInput input;

    private float startingPos;

    void Start()
    {
        input = GetComponent<PlayerInput>();

        startingPos = transform.position.z;
    }
    
    void FixedUpdate()
    {
        var newPos = transform.position;
        newPos.z += boatSpeed * Time.deltaTime;

       if (input.Pause)
        {
            newPos.z = startingPos;
        }

        newPos.x += input.Horizontal * boatStrafeSpeed * Time.deltaTime;

        transform.position = newPos;
    }
}
